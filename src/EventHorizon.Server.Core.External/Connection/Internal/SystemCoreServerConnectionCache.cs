using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.Http.Connections.Client;

namespace EventHorizon.Server.Core.External.Connection.Internal
{
    public class SystemCoreServerConnectionCache : CoreServerConnectionCache, IDisposable
    {
        readonly ILogger _logger;

        // Internal State
        private HubConnection _connection;

        public SystemCoreServerConnectionCache(
            ILogger<SystemCoreServerConnectionCache> logger
        )
        {
            _logger = logger;
        }

        public void Dispose()
        {
            _connection?.DisposeAsync().GetAwaiter().GetResult();
            _connection = null;
        }

        public Task Stop()
        {
            _connection?.StopAsync().GetAwaiter().GetResult();
            _connection = null;
            return Task.CompletedTask;
        }

        public async Task<HubConnection> GetConnection(
            string url,
            Action<HttpConnectionOptions> configureHttpConnection,
            Func<Exception, Task> onClosed
        )
        {
            if (_connection == null)
            {
                try
                {
                    _connection = new HubConnectionBuilder()
                        .WithUrl(
                            url,
                            configureHttpConnection
                        )
                        .Build();
                    _connection.Closed += (ex) =>
                    {
                        _connection = null;
                        // TODO: Add publish of Connection Closed Event
                        return Task.CompletedTask;
                    };
                    _connection.Closed += onClosed;
                    await _connection.StartAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "Error connecting to player hub"
                    );
                    _connection = null;
                    // TODO: Add publish of Connection Exception Event
                    throw ex;
                }
            }
            return _connection;
        }
    }
}