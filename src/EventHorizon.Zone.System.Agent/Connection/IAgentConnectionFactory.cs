namespace EventHorizon.Zone.System.Agent.Connection
{
    using global::System.Threading.Tasks;

    public interface IAgentConnectionFactory
    {
        Task<IAgentConnection> GetConnection();
    }
}
