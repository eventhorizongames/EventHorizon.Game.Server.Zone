namespace EventHorizon.Zone.System.ClientAssets.Load
{
    using EventHorizon.Zone.Core.Events.FileService;
    using EventHorizon.Zone.Core.Model.Command;
    using EventHorizon.Zone.Core.Model.FileService;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.Core.Model.Json;
    using EventHorizon.Zone.System.ClientAssets.Add;
    using EventHorizon.Zone.System.ClientAssets.Model;

    using global::System.Collections.Generic;
    using global::System.IO;
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    using Microsoft.Extensions.Logging;

    public class LoadSystemClientAssetsCommandHandler
        : IRequestHandler<LoadSystemClientAssetsCommand, StandardCommandResult>
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly IJsonFileLoader _fileLoader;
        private readonly ServerInfo _serverInfo;

        public LoadSystemClientAssetsCommandHandler(
            ILogger<LoadSystemClientAssetsCommandHandler> logger,
            IMediator mediator,
            IJsonFileLoader fileLoader,
            ServerInfo serverInfo
        )
        {
            _logger = logger;
            _mediator = mediator;
            _fileLoader = fileLoader;
            _serverInfo = serverInfo;
        }

        public async Task<StandardCommandResult> Handle(
            LoadSystemClientAssetsCommand notification,
            CancellationToken cancellationToken
        )
        {
            await _mediator.Send(
                new ProcessFilesRecursivelyFromDirectory(
                     Path.Combine(
                        _serverInfo.ClientPath,
                        "Assets"
                    ),
                    OnProcessFile,
                    new Dictionary<string, object>()
                ),
                cancellationToken
            );

            return new();
        }

        private async Task OnProcessFile(
            StandardFileInfo fileInfo,
            IDictionary<string, object> _
        )
        {
            var clientAsset = await _fileLoader.GetFile<ClientAsset>(
                fileInfo.FullName
            );
            if (clientAsset.IsNull())
            {
                _logger.LogError(
                    "Client Asset file was invalid. FileFull={FileFullName}",
                    fileInfo.FullName
                );
                return;
            }

            clientAsset.SetFileFullName(
                fileInfo.FullName
            );

            await _mediator.Publish(
                new AddClientAssetEvent(
                    clientAsset
                )
            );
        }
    }
}
