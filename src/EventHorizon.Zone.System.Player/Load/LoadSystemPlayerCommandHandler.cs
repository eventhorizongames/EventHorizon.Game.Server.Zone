namespace EventHorizon.Zone.System.Player.Load
{
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.Core.Model.Json;
    using EventHorizon.Zone.System.Player.Api;
    using EventHorizon.Zone.System.Player.Model;

    using global::System.IO;
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    public class LoadSystemPlayerCommandHandler
        : IRequestHandler<LoadSystemPlayerCommand, LoadSystemPlayerResult>
    {
        private readonly IJsonFileLoader _fileLoader;
        private readonly ServerInfo _serverInfo;
        private readonly PlayerConfigurationState _state;

        public LoadSystemPlayerCommandHandler(
            IJsonFileLoader fileLoader,
            ServerInfo serverInfo,
            PlayerConfigurationState state
        )
        {
            _fileLoader = fileLoader;
            _serverInfo = serverInfo;
            _state = state;
        }

        public async Task<LoadSystemPlayerResult> Handle(
            LoadSystemPlayerCommand request,
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
                return "player_configuration_not_found";
            }

            var (updated, _) = await _state.Set(
                config,
                cancellationToken
            );

            return new LoadSystemPlayerResult(
                updated,
                updated
                    ? "player_configuration_changed"
                    : string.Empty
            );
        }
    }
}
