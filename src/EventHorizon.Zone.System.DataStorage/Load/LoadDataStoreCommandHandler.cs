namespace EventHorizon.Zone.System.DataStorage.Load
{
    using EventHorizon.Zone.Core.Model.Command;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.Core.Model.Json;
    using EventHorizon.Zone.System.DataStorage.Api;
    using EventHorizon.Zone.System.DataStorage.Model;

    using global::System.Collections.Generic;
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    public class LoadDataStoreCommandHandler
        : IRequestHandler<LoadDataStoreCommand, StandardCommandResult>
    {
        private readonly ServerInfo _serverInfo;
        private readonly IJsonFileLoader _fileLoader;
        private readonly DataStoreManagement _dataStoreManagement;

        public LoadDataStoreCommandHandler(
            ServerInfo serverInfo,
            IJsonFileLoader fileLoader,
            DataStoreManagement dataStoreManagement
        )
        {
            _serverInfo = serverInfo;
            _fileLoader = fileLoader;
            _dataStoreManagement = dataStoreManagement;
        }

        public async Task<StandardCommandResult> Handle(
            LoadDataStoreCommand request,
            CancellationToken cancellationToken
        )
        {
            var (_, fileFullName) = DataStorageLocation.GenerateDataStorageLocation(
                _serverInfo.AppDataPath
            );
            var dataStore = await _fileLoader.GetFile<Dictionary<string, object>>(
                fileFullName
            );

            if (dataStore.IsNotNull())
            {
                _dataStoreManagement.Set(
                    dataStore
                );
            }

            return new();
        }
    }
}
