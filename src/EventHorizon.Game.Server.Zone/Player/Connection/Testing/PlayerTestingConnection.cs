namespace EventHorizon.Game.Server.Core.Player.Connection.Testing
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    using EventHorizon.Zone.Core.Model.Json;
    using EventHorizon.Zone.Core.Model.ServerProperty;
    using EventHorizon.Zone.System.Player.Connection;
    using EventHorizon.Zone.System.Player.Model.Details;

    using Microsoft.Extensions.Logging;

    public class PlayerTestingConnection
        : PlayerServerConnection
    {
        readonly IServerProperty _serverProperty;
        readonly IJsonFileLoader _fileLoader;

        public PlayerTestingConnection(
            IServerProperty serverProperty,
            IJsonFileLoader fileLoader
        )
        {
            _serverProperty = serverProperty;
            _fileLoader = fileLoader;
        }

        public void OnAction<T>(string actionName, Action<T> action)
        {
        }

        public void OnAction(string actionName, Action action)
        {
        }

        public async Task<T?> SendAction<T>(string actionName, object[] args)
        {
            if ("GetPlayer".Equals(actionName))
            {
                dynamic playerEntity = await CreateTestEntity((string)args[0]);
                return playerEntity;
            }
            return default;
        }

        public Task SendAction(string actionName, object[] args)
        {
            return Task.CompletedTask;
        }

        private async Task<PlayerDetails> CreateTestEntity(
            string id
        )
        {
            var player = await _fileLoader.GetFile<PlayerDetails>(
                Path.Combine(
                    "App_Data",
                    "Testing.Player.json"
                )
            );
            player.Id = id;
            var locationState = player.Location;
            locationState.CurrentZone = _serverProperty.Get<string>(
                ServerPropertyKeys.SERVER_ID
            );
            player.Location = locationState;

            return player;
        }
    }
}
