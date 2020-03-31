namespace EventHorizon.Zone.System.ClientEntities.Save
{
    using MediatR;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using EventHorizon.Zone.Core.Events.FileService;
    using EventHorizon.Zone.System.Backup.Events;
    using global::System.IO;
    using global::System;
    using EventHorizon.Zone.Core.Model.Json;
    using EventHorizon.Zone.System.ClientEntities.Register;
    using EventHorizon.Zone.System.Agent.Save.Mapper;

    public class SaveClientEntityCommandHandler : IRequestHandler<SaveClientEntityCommand>
    {
        private readonly IJsonFileSaver _fileSaver;
        private readonly IMediator _mediator;

        public SaveClientEntityCommandHandler(
            IMediator mediator,
            IJsonFileSaver fileSaver
        )
        {
            _mediator = mediator;
            _fileSaver = fileSaver;
        }

        public async Task<Unit> Handle(
            SaveClientEntityCommand request,
            CancellationToken cancellationToken
        )
        {
            try
            {
                var fileFullName = (string)request.ClientEntity.Data["editor:Metadata:FullName"];
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
                    request.ClientEntity
                );

                await _mediator.Send(
                    new RegisterClientEntityCommand(
                        ClientEntityFromDetailsToEntity.Map(
                            request.ClientEntity
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