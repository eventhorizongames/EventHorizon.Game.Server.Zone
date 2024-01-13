namespace EventHorizon.Zone.System.Player.Connection;

using global::System.Threading.Tasks;

public interface PlayerServerConnectionFactory
{
    Task<PlayerServerConnection> GetConnection();
}
