namespace EventHorizon.Zone.System.Player.Load;

using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.Core.Model.Json;
using EventHorizon.Zone.System.Player.Api;
using EventHorizon.Zone.System.Player.Model;

using global::System.Collections.Generic;
using global::System.IO;
using global::System.Linq;
using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

public class LoadPlayerSystemCommandHandler
    : IRequestHandler<LoadPlayerSystemCommand, LoadPlayerSystemResult>
{
    private readonly IJsonFileLoader _fileLoader;
    private readonly ServerInfo _serverInfo;
    private readonly PlayerSettingsState _state;

    public LoadPlayerSystemCommandHandler(
        IJsonFileLoader fileLoader,
        ServerInfo serverInfo,
        PlayerSettingsState state
    )
    {
        _fileLoader = fileLoader;
        _serverInfo = serverInfo;
        _state = state;
    }

    public async Task<LoadPlayerSystemResult> Handle(
        LoadPlayerSystemCommand request,
        CancellationToken cancellationToken
    )
    {
        var (ConfigIsError, ConfigUpdated, ConfigReason) = await LoadPlayerConfiguration(
            cancellationToken
        );
        if (ConfigIsError)
        {
            return new LoadPlayerSystemResult(
                ConfigReason
            );
        }

        var (DataIsError, DataUpdated, DataReason) = await LoadPlayerData(
            cancellationToken
        );
        if (DataIsError)
        {
            return new LoadPlayerSystemResult(
                DataReason
            );
        }

        return new LoadPlayerSystemResult(
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

    private async Task<(bool IsError, bool Updated, string Reason)> LoadPlayerConfiguration(
        CancellationToken cancellationToken
    )
    {
        var fileFullName = Path.Combine(
            _serverInfo.AppDataPath,
            PlayerSystemConstants.PlayerAppDataPath,
            PlayerSystemConstants.PlayerConfigurationFileName
        );

        var config = await _fileLoader.GetFile<PlayerObjectEntityConfigurationModel>(
            fileFullName
        );

        if (config is null)
        {
            return (true, false, "player_configuration_not_found");
        }

        var (updated, _) = await _state.SetConfiguration(
            config,
            cancellationToken
        );

        return (
            false,
            updated,
            updated
                ? "player_configuration_changed"
                : string.Empty
        );
    }
    private async Task<(bool IsError, bool Updated, string Reason)> LoadPlayerData(
        CancellationToken cancellationToken
    )
    {
        var fileFullName = Path.Combine(
            _serverInfo.AppDataPath,
            PlayerSystemConstants.PlayerAppDataPath,
            PlayerSystemConstants.PlayerDataFileName
        );

        var config = await _fileLoader.GetFile<PlayerObjectEntityDataModel>(
            fileFullName
        );

        if (config is null)
        {
            return (true, false, "player_data_not_found");
        }

        var (updated, _) = await _state.SetData(
            config,
            cancellationToken
        );

        return (
            false,
            updated,
            updated
                ? "player_data_changed"
                : string.Empty
        );
    }
}
