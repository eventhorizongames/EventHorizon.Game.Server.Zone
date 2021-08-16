using System.Threading.Tasks;

namespace EventHorizon.Zone.System.Agent.Connection
{
    public interface IAgentConnectionFactory
    {
        Task<IAgentConnection> GetConnection();
    }
}
