namespace EventHorizon.Zone.Core.Entity.Load
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using EventHorizon.Zone.Core.Entity.Api;
    using EventHorizon.Zone.Core.Entity.Model;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.Core.Model.Json;

    using MediatR;

    public class LoadEntityCoreCommandHandler
        : IRequestHandler<LoadEntityCoreCommand, LoadEntityCoreResult>
    {
        private readonly IJsonFileLoader _fileLoader;
        private readonly ServerInfo _serverInfo;
        private readonly EntitySettingsState _state;

        public LoadEntityCoreCommandHandler(
            IJsonFileLoader fileLoader,
            ServerInfo serverInfo,
            EntitySettingsState state
        )
        {
            _fileLoader = fileLoader;
            _serverInfo = serverInfo;
            _state = state;
        }

        public async Task<LoadEntityCoreResult> Handle(
            LoadEntityCoreCommand request,
            CancellationToken cancellationToken
        )
        {
            var (ConfigIsError, ConfigUpdated, ConfigReason) = await LoadEntityConfiguration(
                cancellationToken
            );
            if (ConfigIsError)
            {
                return new LoadEntityCoreResult(
                    ConfigReason
                );
            }

            var (DataIsError, DataUpdated, DataReason) = await LoadPlayerData(
                cancellationToken
            );
            if (DataIsError)
            {
                return new LoadEntityCoreResult(
                    DataReason
                );
            }

            return new LoadEntityCoreResult(
                ConfigUpdated || DataUpdated,
                new List<string>
                {
                    ConfigReason,
                    DataReason,
                }.Where(
                    reason => !string.IsNullOrWhiteSpace(reason)
                ).ToArray()
            );
        }

        private async Task<(bool IsError, bool Updated, string Reason)> LoadEntityConfiguration(
            CancellationToken cancellationToken
        )
        {
            var fileFullName = Path.Combine(
                _serverInfo.AppDataPath,
                EntityCoreConstants.EntityAppDataPath,
                EntityCoreConstants.EntityConfigurationFileName
            );

            var config = await _fileLoader.GetFile<ObjectEntityConfigurationModel>(
                fileFullName
            );

            if (config is null)
            {
                return (true, false, "entity_configuration_not_found");
            }

            var (updated, _) = await _state.SetConfiguration(
                config,
                cancellationToken
            );

            return (
                false,
                updated,
                updated
                    ? "entity_configuration_changed"
                    : string.Empty
            );
        }
        private async Task<(bool IsError, bool Updated, string Reason)> LoadPlayerData(
            CancellationToken cancellationToken
        )
        {
            var fileFullName = Path.Combine(
                _serverInfo.AppDataPath,
                EntityCoreConstants.EntityAppDataPath,
                EntityCoreConstants.EntityDataFileName
            );

            var config = await _fileLoader.GetFile<ObjectEntityDataModel>(
                fileFullName
            );

            if (config is null)
            {
                return (true, false, "entity_data_not_found");
            }

            var (updated, _) = await _state.SetData(
                config,
                cancellationToken
            );

            return (
                false,
                updated,
                updated
                    ? "entity_data_changed"
                    : string.Empty
            );
        }
    }
}
