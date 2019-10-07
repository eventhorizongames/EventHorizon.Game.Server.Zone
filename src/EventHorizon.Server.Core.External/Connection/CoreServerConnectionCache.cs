using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Connections.Client;
using Microsoft.AspNetCore.SignalR.Client;

namespace EventHorizon.Server.Core.External.Connection
{
    public interface CoreServerConnectionCache
    {
        Task Stop();
        Task<HubConnection> GetConnection(
            string url, 
            Action<HttpConnectionOptions> configureHttpConnection
        );
    }
}