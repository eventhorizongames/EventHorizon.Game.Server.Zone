namespace EventHorizon.Server.Core.Connection.Internal
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http.Connections.Client;
    using Microsoft.AspNetCore.SignalR.Client;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    public class SystemCoreServerConnectionCache
        : CoreServerConnectionCache,
        IAsyncDisposable
    {
        private readonly ILogger _logger;
        private readonly SemaphoreSlim _lock;

        private HubConnection? _connection;
        private string _lastConnectionId = "not-set";

        public SystemCoreServerConnectionCache(
            ILogger<SystemCoreServerConnectionCache> logger
        )
        {
            _logger = logger;
            _lock = new(1, 1);
        }

        public async ValueTask DisposeAsync()
        {
            await Stop();
        }

        public async Task Stop()
        {
            if (_connection == null)
            {
                return;
            }

            await _connection.StopAsync();
            _connection = null;
        }

        public async Task<HubConnection> GetConnection(
            string url,
            Action<HttpConnectionOptions> configureHttpConnection,
            Func<Exception, Task> onClosed
        )
        {
            if (_connection == null)
            {
                await _lock.WaitAsync();
                try
                {
                    if (_connection != null)
                    {
                        return _connection;
                    }
                    _logger.LogWarning(
                        "Creating new Core Server Connection"
                    );
                    _connection = new HubConnectionBuilder()
                        .AddNewtonsoftJsonProtocol()
                        .WithUrl(
                            url,
                            configureHttpConnection
                        ).WithAutomaticReconnect()
                        .Build();
                    _connection.Closed += (ex) =>
                    {
                        _logger.LogWarning(
                            "Core Server {ConnectionId} Connection Closed.",
                            _lastConnectionId
                        );
                        // TODO: Add publish of Connection Closed Event
                        return Task.CompletedTask;
                    };
                    _connection.Closed += onClosed;
                    await _connection.StartAsync();

                    _logger.LogWarning(
                        "Created new Core Server Connection of {ConnectionId}",
                        _connection.ConnectionId
                    );
                    _lastConnectionId = _connection.ConnectionId;
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "Error connecting to Core hub"
                    );
                    _connection = null;
                    // TODO: Add publish of Connection Exception Event
                    throw;
                }
                finally
                {
                    _lock.Release();
                }
            }

            return _connection;
        }
    }
}
