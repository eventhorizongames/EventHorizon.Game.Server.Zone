using System;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Core.Player.Model;
using EventHorizon.Game.Server.Zone.Core.ServerProperty;
using EventHorizon.Zone.Core.Model.Json;
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
        readonly IServerProperty _serverProperty;
        readonly IJsonFileLoader _fileLoader;

        public PlayerTestingConnectionFactory(
            ILoggerFactory loggerFactory,
            IServerProperty serverProperty,
            IJsonFileLoader fileLoader)
        {
            _logger = loggerFactory.CreateLogger<PlayerConnectionFactory>();

            _loggerFactory = loggerFactory;
            _serverProperty = serverProperty;
            _fileLoader = fileLoader;
        }

        public Task<IPlayerConnection> GetConnection()
        {
            return Task.FromResult(
                new PlayerTestingConnection(
                    _loggerFactory.CreateLogger<PlayerTestingConnection>(),
                    _serverProperty,
                    _fileLoader
                ) as IPlayerConnection);
        }
    }
}