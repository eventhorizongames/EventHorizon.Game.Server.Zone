namespace EventHorizon.Game.Server.Core.Player.Connection.Testing;

using System.Threading.Tasks;

using EventHorizon.Zone.Core.Model.Json;
using EventHorizon.Zone.Core.Model.ServerProperty;
using EventHorizon.Zone.System.Player.Connection;

public class PlayerTestingConnectionFactory
    : PlayerServerConnectionFactory
{
    readonly IServerProperty _serverProperty;
    readonly IJsonFileLoader _fileLoader;

    public PlayerTestingConnectionFactory(
        IServerProperty serverProperty,
        IJsonFileLoader fileLoader
    )
    {

        _serverProperty = serverProperty;
        _fileLoader = fileLoader;
    }

    public Task<PlayerServerConnection> GetConnection()
    {
        return Task.FromResult(
            new PlayerTestingConnection(
                _serverProperty,
                _fileLoader
            ) as PlayerServerConnection
        );
    }
}
