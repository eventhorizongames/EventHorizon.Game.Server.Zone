namespace EventHorizon.Zone.System.ClientEntities.Delete
{
    using EventHorizon.Zone.Core.Events.FileService;
    using EventHorizon.Zone.System.Backup.Events;
    using EventHorizon.Zone.System.ClientEntities.Client.Delete;
    using EventHorizon.Zone.System.ClientEntities.Model;
    using EventHorizon.Zone.System.ClientEntities.Model.Client;
    using EventHorizon.Zone.System.ClientEntities.State;
    using EventHorizon.Zone.System.ClientEntities.Unregister;

    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    public class DeleteClientEntityCommandHandler
        : IRequestHandler<DeleteClientEntityCommand, DeleteClientEntityResponse>
    {
        private readonly IMediator _mediator;
        private readonly ClientEntityRepository _repository;

        public DeleteClientEntityCommandHandler(
            IMediator mediator,
            ClientEntityRepository repository
        )
        {
            _mediator = mediator;
            _repository = repository;
        }

        public async Task<DeleteClientEntityResponse> Handle(
            DeleteClientEntityCommand request,
            CancellationToken cancellationToken
        )
        {
            var clientEntity = _repository.Find(
                request.ClientEntityId
            );

            if (!clientEntity.IsFound())
            {
                return new DeleteClientEntityResponse(
                    ClientEntityErrorCodes.NOT_FOUND
                );
            }

            // This will also trigger a reload
            var deleteErrorCode = await DeleteClientEntityFile(
                clientEntity
            );

            if (!string.IsNullOrWhiteSpace(deleteErrorCode))
            {
                return new DeleteClientEntityResponse(
                    deleteErrorCode
                );
            }

            // Unregister and Delete Entity from Client
            await _mediator.Send(
                new UnregisterClientEntity(
                    clientEntity.GlobalId
                )
            );

            await _mediator.Publish(
                SendClientEntityDeletedClientActionToAllEvent.Create(
                    new ClientEntityDeletedClientActionData(
                        clientEntity.GlobalId
                    )
                )
            );

            return new DeleteClientEntityResponse(
                true
            );
        }

        private async Task<string> DeleteClientEntityFile(
            ClientEntity clientEntity
        )
        {
            var fileFullName = clientEntity.RawData[ClientEntityConstants.METADATA_FILE_FULL_NAME] as string;
            if(string.IsNullOrWhiteSpace(
                fileFullName
            ))
            {
                return ClientEntityErrorCodes.FILE_NOT_FOUND;
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
                // Run a Backup
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
                // Delete the File
                await _mediator.Send(
                    new DeleteFile(
                        fileInfo.FullName
                    )
                );
                // Empty string means successful delete
                return string.Empty;
            }

            return ClientEntityErrorCodes.FILE_NOT_FOUND;
        }
    }
}
