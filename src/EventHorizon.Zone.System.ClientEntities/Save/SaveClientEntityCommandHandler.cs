namespace EventHorizon.Zone.System.ClientEntities.Save
{
    using MediatR;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using EventHorizon.Zone.Core.Events.FileService;
    using EventHorizon.Zone.System.Backup.Events;
    using global::System;
    using EventHorizon.Zone.Core.Model.Json;
    using EventHorizon.Zone.System.ClientEntities.Register;
    using EventHorizon.Zone.System.Agent.Save.Mapper;
    using EventHorizon.Zone.System.ClientEntities.Unregister;
    using EventHorizon.Zone.System.ClientEntities.Client;
    using EventHorizon.Zone.System.ClientEntities.Model.Client;
    using EventHorizon.Zone.System.ClientEntities.State;

    public class SaveClientEntityCommandHandler : IRequestHandler<SaveClientEntityCommand>
    {
        private readonly IMediator _mediator;
        private readonly IJsonFileSaver _fileSaver;
        private readonly ClientEntityRepository _repository;

        public SaveClientEntityCommandHandler(
            IMediator mediator,
            IJsonFileSaver fileSaver,
            ClientEntityRepository repository
        )
        {
            _mediator = mediator;
            _fileSaver = fileSaver;
            _repository = repository;
        }

        public async Task<Unit> Handle(
            SaveClientEntityCommand request,
            CancellationToken cancellationToken
        )
        {
            try
            {
                request.ClientEntity.Data.Clear();
                var fileFullName = request.ClientEntity.RawData["editor:Metadata:FullName"] as string;
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
                    new SendClientEntityChangedClientActionToAllEvent(
                        new ClientEntityChangedClientActionData(
                            registeredClientEntity
                        )
                    )
                );
            }
            catch (Exception)
            {
                throw;
            }
            return Unit.Value;
        }
    }
}