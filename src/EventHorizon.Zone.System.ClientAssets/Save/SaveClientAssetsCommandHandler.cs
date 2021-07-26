namespace EventHorizon.Zone.System.ClientAssets.Save
{
    using EventHorizon.Performance;
    using EventHorizon.Zone.Core.Model.Command;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.Core.Model.Json;
    using EventHorizon.Zone.System.ClientAssets.Model;
    using EventHorizon.Zone.System.ClientAssets.State.Api;
    using global::System.IO;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using MediatR;

    public class SaveClientAssetsCommandHandler
        : IRequestHandler<SaveClientAssetsCommand, StandardCommandResult>
    {
        private readonly PerformanceTrackerFactory _performanceTrackerFactory;
        private readonly ServerInfo _serverInfo;
        private readonly IJsonFileSaver _fileSaver;
        private readonly ClientAssetRepository _repository;

        public SaveClientAssetsCommandHandler(
            PerformanceTrackerFactory performanceTrackerFactory,
            ServerInfo serverInfo,
            IJsonFileSaver fileSaver,
            ClientAssetRepository repository
        )
        {
            _performanceTrackerFactory = performanceTrackerFactory;
            _serverInfo = serverInfo;
            _fileSaver = fileSaver;
            _repository = repository;
        }

        public async Task<StandardCommandResult> Handle(
            SaveClientAssetsCommand request,
            CancellationToken cancellationToken
        )
        {
            using var _ = _performanceTrackerFactory.Build(nameof(SaveClientAssetsCommandHandler));
            foreach (var clientAsset in _repository.All())
            {
                if (!clientAsset.TryGetFileFullName(
                    out var fileFullName
                ))
                {
                    fileFullName = clientAsset.GetFileFullName(
                        _serverInfo.ClientPath
                    );
                    clientAsset.SetFileFullName(
                        fileFullName
                    );
                }

                var directory = Path.GetDirectoryName(
                    fileFullName
                );
                var fileName = Path.GetFileName(
                    fileFullName
                );

                await _fileSaver.SaveToFile(
                    directory,
                    fileName,
                    clientAsset
                );
            }


            return new();
        }
    }
}
