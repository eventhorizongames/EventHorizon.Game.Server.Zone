using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Connections.Client;
using Microsoft.AspNetCore.SignalR.Client;

namespace EventHorizon.Game.Server.Zone.Core.Connection
{
    public interface ICoreConnectionCache
    {
        Task Stop();
        Task<HubConnection> GetConnection(string url, Action<HttpConnectionOptions> configureHttpConnection);
    }
}