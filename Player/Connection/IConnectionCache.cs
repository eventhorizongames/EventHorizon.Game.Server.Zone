using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Connections.Client;
using Microsoft.AspNetCore.SignalR.Client;

namespace EventHorizon.Game.Server.Core.Player.Connection
{
    public interface IConnectionCache
    {
         Task<HubConnection> GetConnection(string url, Action<HttpConnectionOptions> configureHttpConnection);
    }
}