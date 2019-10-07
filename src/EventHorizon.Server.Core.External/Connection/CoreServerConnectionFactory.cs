using System.Threading.Tasks;

namespace EventHorizon.Server.Core.External.Connection
{
    public interface CoreServerConnectionFactory
    {
        Task<CoreServerConnection> GetConnection();
    }
}