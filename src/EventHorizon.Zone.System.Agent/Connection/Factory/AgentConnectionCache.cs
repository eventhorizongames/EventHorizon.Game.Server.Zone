namespace EventHorizon.Zone.System.Agent.Connection.Factory
{
    using global::System;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using Microsoft.AspNetCore.Http.Connections.Client;
    using Microsoft.AspNetCore.SignalR.Client;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    public class AgentConnectionCache : IAgentConnectionCache, IAsyncDisposable
    {
        private readonly ILogger _logger;
        private readonly SemaphoreSlim _lock;

        private HubConnection? _connection;
        private string _lastConnectionId = "not-set";

        public AgentConnectionCache(ILogger<AgentConnectionCache> logger)
        {
            _logger = logger;
            _lock = new SemaphoreSlim(1, 1);
        }

        public async ValueTask DisposeAsync()
        {
            if (_connection != null)
            {
                await _connection.DisposeAsync();
                _connection = null;
            }
        }

        public async Task<HubConnection> GetConnection(
            string url,
            Action<HttpConnectionOptions> configureHttpConnection
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

                    _connection = new HubConnectionBuilder()
                        .AddNewtonsoftJsonProtocol()
                        .WithUrl(url, configureHttpConnection)
                        .Build();
                    _connection.Closed += (ex) =>
                    {
                        _logger.LogWarning(
                            "Agent Server {ConnectionId} Connection Closed.",
                            _lastConnectionId
                        );
                        _connection = null;

                        return Task.CompletedTask;
                    };

                    _logger.LogWarning(
                        "Created new Agent Server Connection of {ConnectionId}",
                        _connection.ConnectionId
                    );
                    await _connection.StartAsync();
                    _lastConnectionId = _connection.ConnectionId ?? "not-set";
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "Error connecting to Agent hub: {AgentConnectionUrl}",
                        url
                    );
                    _connection = null;
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
