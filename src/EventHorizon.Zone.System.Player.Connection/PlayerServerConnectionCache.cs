namespace EventHorizon.Zone.System.Player.Connection
{
    using global::System;
    using global::System.Threading.Tasks;

    using Microsoft.AspNetCore.Http.Connections.Client;
    using Microsoft.AspNetCore.SignalR.Client;

    public interface PlayerServerConnectionCache
    {
        Task Stop();
        Task<HubConnection> GetConnection(
            string url,
            Action<HttpConnectionOptions> configureHttpConnection
        );
    }
}
