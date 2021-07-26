namespace EventHorizon.Zone.System.ClientAssets.Delete
{
    using EventHorizon.Zone.Core.Events.Entity.Search;
    using EventHorizon.Zone.Core.Events.FileService;
    using EventHorizon.Zone.Core.Model.Command;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.System.Backup.Events;
    using EventHorizon.Zone.System.ClientAssets.Events.Delete;
    using EventHorizon.Zone.System.ClientAssets.Save;
    using EventHorizon.Zone.System.ClientAssets.State.Api;
    using global::System.IO;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using MediatR;

    public class DeleteClientAssetCommandHandler
        : IRequestHandler<DeleteClientAssetCommand, StandardCommandResult>
    {
        private readonly IMediator _mediator;
        private readonly ServerInfo _serverInfo;
        private readonly ClientAssetRepository _repository;

        public DeleteClientAssetCommandHandler(
            IMediator mediator,
            ServerInfo serverInfo,
            ClientAssetRepository repository
        )
        {
            _mediator = mediator;
            _serverInfo = serverInfo;
            _repository = repository;
        }

        public async Task<StandardCommandResult> Handle(
            DeleteClientAssetCommand request,
            CancellationToken cancellationToken
        )
        {
            var clientAssetOption = _repository.Get(
                request.Id
            );
            if (!clientAssetOption)
            {
                // Nothing found, just return success
                return new();
            }
            var clientAsset = clientAssetOption.Value;

            if (clientAsset.TryGetFileFullName(
                out var assetFileFullName
            ))
            {
                await _mediator.Send(
                    new CreateBackupOfFileCommand(
                        Path.Combine(
                            _serverInfo.ClientPath,
                            "Assets"
                        ),
                        assetFileFullName
                    ),
                    cancellationToken
                );

                await _mediator.Send(
                    new DeleteFile(
                        assetFileFullName
                    ),
                    cancellationToken
                );
            }

            _repository.Delete(
                request.Id
            );

            return new();
        }
    }
}
