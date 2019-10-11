using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Connections.Client;
using Microsoft.AspNetCore.SignalR.Client;

namespace EventHorizon.Zone.System.Player.Connection
{
    public interface PlayerServerConnectionCache
    {
        Task Stop();
        Task<HubConnection> GetConnection(
            string url, 
            Action<HttpConnectionOptions> configureHttpConnection
        );
    }
}