using System.Threading.Tasks;
using EventHorizon.Zone.Core.Model.Json;
using Microsoft.Extensions.Logging;
using EventHorizon.Zone.System.Player.Connection;
using EventHorizon.Zone.Core.Model.ServerProperty;

namespace EventHorizon.Game.Server.Core.Player.Connection.Testing
{
    public class PlayerTestingConnectionFactory : PlayerServerConnectionFactory
    {
        readonly ILogger _logger;
        readonly ILoggerFactory _loggerFactory;
        readonly IServerProperty _serverProperty;
        readonly IJsonFileLoader _fileLoader;

        public PlayerTestingConnectionFactory(
            ILoggerFactory loggerFactory,
            IServerProperty serverProperty,
            IJsonFileLoader fileLoader
        )
        {
            _logger = loggerFactory.CreateLogger<PlayerTestingConnectionFactory>();

            _loggerFactory = loggerFactory;
            _serverProperty = serverProperty;
            _fileLoader = fileLoader;
        }

        public Task<PlayerServerConnection> GetConnection()
        {
            return Task.FromResult(
                new PlayerTestingConnection(
                    _loggerFactory.CreateLogger<PlayerTestingConnection>(),
                    _serverProperty,
                    _fileLoader
                ) as PlayerServerConnection
            );
        }
    }
}