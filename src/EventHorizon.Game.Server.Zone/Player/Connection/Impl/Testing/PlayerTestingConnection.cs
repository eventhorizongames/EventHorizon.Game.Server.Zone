using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Core.Player.Model;
using EventHorizon.Game.Server.Zone.Core.ServerProperty;
using EventHorizon.Game.Server.Zone.External.Info;
using EventHorizon.Game.Server.Zone.External.Json;
using EventHorizon.Game.Server.Zone.Model.Core;
using EventHorizon.Game.Server.Zone.Player.Actions.MovePlayer;
using MediatR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using IOPath = System.IO.Path;

namespace EventHorizon.Game.Server.Core.Player.Connection.Impl.Testing
{
    public class PlayerTestingConnection : IPlayerConnection
    {
        readonly ILogger _logger;
        readonly IServerProperty _serverProperty;
        readonly IJsonFileLoader _fileLoader;

        public PlayerTestingConnection(
            ILogger logger,
            IServerProperty serverProperty,
            IJsonFileLoader fileLoader
        )
        {
            _logger = logger;
            _serverProperty = serverProperty;
            _fileLoader = fileLoader;
        }

        public void OnAction<T>(string actionName, Action<T> action)
        {
        }
        public void OnAction(string actionName, Action action)
        {
        }

        public async Task<T> SendAction<T>(string actionName, object[] args)
        {
            if ("GetPlayer".Equals(actionName))
            {
                dynamic playerEntity = await CreateTestEntity((string)args[0]);
                return playerEntity;
            }
            return default(T);
        }

        public Task SendAction(string actionName, object[] args)
        {
            return Task.CompletedTask;
        }

        private async Task<PlayerDetails> CreateTestEntity(string id)
        {
            var player = await _fileLoader.GetFile<PlayerDetails>(
                IOPath.Combine(
                    "App_Data",
                    "Testing.Player.json"
                )
            );
            player.Id = id;
            player.Position.CurrentZone = _serverProperty.Get<string>(ServerPropertyKeys.SERVER_ID);

            return player;
        }
    }
}