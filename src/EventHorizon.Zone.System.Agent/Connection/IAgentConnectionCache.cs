namespace EventHorizon.Zone.System.Agent.Connection
{
    using global::System;
    using global::System.Threading.Tasks;

    using Microsoft.AspNetCore.Http.Connections.Client;
    using Microsoft.AspNetCore.SignalR.Client;

    public interface IAgentConnectionCache
    {
        Task<HubConnection> GetConnection(string url, Action<HttpConnectionOptions> configureHttpConnection);
    }
}
