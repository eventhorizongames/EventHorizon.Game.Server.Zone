using System.Threading.Tasks;

namespace EventHorizon.Game.Server.Zone.Agent.Connection
{
    public interface IAgentConnectionFactory
    {
        Task<IAgentConnection> GetConnection();
    }
}