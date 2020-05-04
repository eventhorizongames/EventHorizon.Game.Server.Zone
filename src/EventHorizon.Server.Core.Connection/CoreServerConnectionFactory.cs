namespace EventHorizon.Server.Core.Connection
{
    using System.Threading.Tasks;

    public interface CoreServerConnectionFactory
    {
        Task<CoreServerConnection> GetConnection();
    }
}