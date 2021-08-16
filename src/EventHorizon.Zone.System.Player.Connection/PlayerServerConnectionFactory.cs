using System.Threading.Tasks;

namespace EventHorizon.Zone.System.Player.Connection
{
    public interface PlayerServerConnectionFactory
    {
        Task<PlayerServerConnection> GetConnection();
    }
}
