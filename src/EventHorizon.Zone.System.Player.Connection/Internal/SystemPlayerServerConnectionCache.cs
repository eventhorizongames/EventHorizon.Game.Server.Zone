namespace EventHorizon.Zone.System.Player.Connection.Internal
{
    using global::System;
    using global::System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.AspNetCore.SignalR.Client;
    using Microsoft.AspNetCore.Http.Connections.Client;
    using Microsoft.Extensions.DependencyInjection;

    public class SystemPlayerServerConnectionCache : PlayerServerConnectionCache, IDisposable
    {
        readonly ILogger _logger;

        // Internal State
        private HubConnection _connection;

        public SystemPlayerServerConnectionCache(
            ILogger<SystemPlayerServerConnectionCache> logger
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
            Action<HttpConnectionOptions> configureHttpConnection
        )
        {
            if (_connection == null)
            {
                try
                {
                    _connection = new HubConnectionBuilder()
                        .AddNewtonsoftJsonProtocol()
                        .WithUrl(
                            url,
                            configureHttpConnection
                        )
                        .Build();
                    _connection.Closed += (ex) =>
                    {
                        _connection = null;
                        // TODO: Add publish of Connection Closed Event
                        return Task.FromResult(0);
                    };
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
                    throw;
                }
            }
            return _connection;
        }
    }
}