using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Core.Player.Model;
using EventHorizon.Game.Server.Zone.Core.ServerProperty;
using EventHorizon.Game.Server.Zone.Model.Core;
using EventHorizon.Game.Server.Zone.Player.Actions.MovePlayer;
using MediatR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EventHorizon.Game.Server.Core.Player.Connection.Impl.Testing
{
    public class PlayerTestingConnection : IPlayerConnection
    {
        readonly ILogger _logger;
        readonly IServerProperty _serverProperty;

        public PlayerTestingConnection(ILogger logger, IServerProperty serverProperty)
        {
            _logger = logger;
            _serverProperty = serverProperty;
        }

        public void OnAction<T>(string actionName, Action<T> action)
        {
        }
        public void OnAction(string actionName, Action action)
        {
        }

        public Task<T> SendAction<T>(string actionName, object[] args)
        {
            if ("GetPlayer".Equals(actionName))
            {
                dynamic playerEntity = CreateTestEntity((string)args[0]);
                return Task.FromResult(playerEntity);
            }
            return Task.FromResult(default(T));
        }

        public Task SendAction(string actionName, object[] args)
        {
            return Task.CompletedTask;
        }

        private PlayerDetails CreateTestEntity(string id)
        {
            return new PlayerDetails
            {
                Id = id,
                Name = "Test_Player",
                Locale = "en_US",
                Position = new PlayerPositionState
                {
                    Position = Vector3.Zero,
                    CurrentZone = _serverProperty.Get<string>(ServerPropertyKeys.SERVER_ID),
                    ZoneTag = "testing",
                },
                Data = new Dictionary<string, object>(),
            };
        }
    }
}