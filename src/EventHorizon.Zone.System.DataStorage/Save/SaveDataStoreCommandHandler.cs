namespace EventHorizon.Zone.System.DataStorage.Save
{
    using EventHorizon.Zone.Core.Model.Command;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.Core.Model.Json;
    using EventHorizon.Zone.System.DataStorage.Api;
    using EventHorizon.Zone.System.DataStorage.Model;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using MediatR;

    public class SaveDataStoreCommandHandler
        : IRequestHandler<SaveDataStoreCommand, StandardCommandResult>
    {
        private readonly ServerInfo _serverInfo;
        private readonly IJsonFileSaver _fileSaver;
        private readonly DataStoreManagement _dataStoreManagement;

        public SaveDataStoreCommandHandler(
            ServerInfo serverInfo,
            IJsonFileSaver fileSaver,
            DataStoreManagement dataStoreManagement
        )
        {
            _serverInfo = serverInfo;
            _fileSaver = fileSaver;
            _dataStoreManagement = dataStoreManagement;
        }


        public async Task<StandardCommandResult> Handle(
            SaveDataStoreCommand request, CancellationToken cancellationToken
        )
        {
            var (saveDirectory, saveFile) = DataStorageLocation.GenerateDataStorageLocation(
                _serverInfo.AppDataPath
            );
            var data = _dataStoreManagement.Data();
            await _fileSaver.SaveToFile(
                saveDirectory,
                saveFile,
                data
            );

            return new();
        }
    }
}
