using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http.Connections.Client;
using Microsoft.AspNetCore.SignalR.Client;

namespace EventHorizon.Zone.System.Agent.Connection
{
    public interface IAgentConnectionCache
    {
        Task<HubConnection> GetConnection(string url, Action<HttpConnectionOptions> configureHttpConnection);
    }
}
