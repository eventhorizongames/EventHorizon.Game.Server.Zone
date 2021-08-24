namespace EventHorizon.Zone.System.ClientEntities.Save
{
    using EventHorizon.Zone.Core.Events.FileService;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.Core.Model.Json;
    using EventHorizon.Zone.System.Agent.Save.Mapper;
    using EventHorizon.Zone.System.Backup.Events;
    using EventHorizon.Zone.System.ClientEntities.Client;
    using EventHorizon.Zone.System.ClientEntities.Model;
    using EventHorizon.Zone.System.ClientEntities.Model.Client;
    using EventHorizon.Zone.System.ClientEntities.Register;
    using EventHorizon.Zone.System.ClientEntities.State;
    using EventHorizon.Zone.System.ClientEntities.Unregister;

    using global::System;
    using global::System.IO;
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    public class SaveClientEntityCommandHandler : IRequestHandler<SaveClientEntityCommand, SaveClientEntityResponse>
    {
        private readonly IMediator _mediator;
        private readonly ServerInfo _serverInfo;
        private readonly IJsonFileSaver _fileSaver;
        private readonly ClientEntityRepository _repository;

        public SaveClientEntityCommandHandler(
            IMediator mediator,
            ServerInfo serverInfo,
            IJsonFileSaver fileSaver,
            ClientEntityRepository repository
        )
        {
            _mediator = mediator;
            _serverInfo = serverInfo;
            _fileSaver = fileSaver;
            _repository = repository;
        }

        public async Task<SaveClientEntityResponse> Handle(
            SaveClientEntityCommand request,
            CancellationToken cancellationToken
        )
        {
            try
            {
                request.ClientEntity.Data.Clear();
                var fileFullName = request.ClientEntity.RawData[ClientEntityConstants.METADATA_FILE_FULL_NAME] as string;
                if (string.IsNullOrWhiteSpace(
                    fileFullName
                ))
                {
                    fileFullName = Path.Combine(
                        _serverInfo.ClientEntityPath,
                        $"{request.ClientEntity.ClientEntityId}.json"
                    );
                }

                var fileInfo = await _mediator.Send(
                    new GetFileInfo(
                        fileFullName
                    )
                );
                if (await _mediator.Send(
                    new DoesFileExist(
                        fileInfo.FullName
                    )
                ))
                {
                    await _mediator.Send(
                        new CreateBackupOfFileContentCommand(
                            new string[] { "Client", "Entity" },
                            fileInfo.Name,
                            await _mediator.Send(
                                new ReadAllTextFromFile(
                                    fileInfo.FullName
                                )
                            )
                        )
                    );
                }

                await _fileSaver.SaveToFile(
                    fileInfo.DirectoryName,
                    fileInfo.Name,
                    ClientEntityFromEntityToDetails.Map(
                        request.ClientEntity
                    )
                );

                await _mediator.Send(
                    new UnregisterClientEntity(
                        request.ClientEntity.GlobalId
                    )
                );

                await _mediator.Send(
                    new RegisterClientEntityCommand(
                        request.ClientEntity
                    )
                );

                var registeredClientEntity = _repository.Find(
                    request.ClientEntity.GlobalId
                );

                await _mediator.Publish(
                    SendClientEntityChangedClientActionToAllEvent.Create(
                        new ClientEntityChangedClientActionData(
                            registeredClientEntity
                        )
                    )
                );
                return new SaveClientEntityResponse(
                    true,
                    registeredClientEntity
                );
            }
            catch (Exception)
            {
                return new SaveClientEntityResponse(
                    "exception"
                );
            }
        }
    }
}
