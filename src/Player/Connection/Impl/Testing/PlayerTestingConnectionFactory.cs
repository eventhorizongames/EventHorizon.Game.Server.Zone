using System;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Core.Player.Model;
using EventHorizon.Identity;
using MediatR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EventHorizon.Game.Server.Core.Player.Connection.Impl.Testing
{
    public class PlayerTestingConnectionFactory : IPlayerConnectionFactory
    {
        readonly ILogger _logger;
        readonly ILoggerFactory _loggerFactory;

        public PlayerTestingConnectionFactory(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<PlayerConnectionFactory>();

            _loggerFactory = loggerFactory;
        }

        public Task<IPlayerConnection> GetConnection()
        {
            return Task.FromResult(new PlayerTestingConnection(_loggerFactory.CreateLogger<PlayerTestingConnection>()) as IPlayerConnection);
        }
    }
}