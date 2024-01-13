namespace EventHorizon.Server.Core.Connection;

using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http.Connections.Client;
using Microsoft.AspNetCore.SignalR.Client;

public interface CoreServerConnectionCache
{
    Task Stop();
    Task<HubConnection> GetConnection(
        string url,
        Action<HttpConnectionOptions> configureHttpConnection,
        Func<Exception, Task> onClosed
    );
}
