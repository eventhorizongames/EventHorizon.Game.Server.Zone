using System;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Core.Player.Model;
using MediatR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EventHorizon.Game.Server.Core.Player.Connection.Impl.Testing
{
    public class PlayerTestingConnection : IPlayerConnection
    {
        readonly ILogger _logger;

        public PlayerTestingConnection(ILogger logger)
        {
            _logger = logger;
        }

        public void OnAction<T>(string actionName, Action<T> action)
        {
        }
        public void OnAction(string actionName, Action action)
        {
        }

        public Task<T> SendAction<T>(string actionName, object[] args)
        {
            return Task.FromResult(default(T));
        }

        public Task SendAction(string actionName, object[] args)
        {
            return Task.CompletedTask;
        }
    }
}