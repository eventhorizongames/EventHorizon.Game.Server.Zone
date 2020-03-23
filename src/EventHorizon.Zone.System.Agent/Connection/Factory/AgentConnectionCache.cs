namespace EventHorizon.Zone.System.Agent.Connection.Factory
{
    using global::System;
    using global::System.Threading.Tasks;
    using Microsoft.AspNetCore.Http.Connections.Client;
    using Microsoft.AspNetCore.SignalR.Client;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.DependencyInjection;

    public class AgentConnectionCache : IAgentConnectionCache, IDisposable
    {
        private readonly ILogger _logger;

        private HubConnection _connection;

        public AgentConnectionCache(
            ILogger<AgentConnectionCache> logger
        )
        {
            _logger = logger;
        }

        public void Dispose()
        {
            _connection?.DisposeAsync().GetAwaiter().GetResult();
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
                        .WithUrl(url, configureHttpConnection)
                        .Build();
                    _connection.Closed += (ex) =>
                    {
                        _connection = null;
                        return Task.FromResult(0);
                    };
                    await _connection.StartAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error connecting to player hub");
                    _connection = null;
                    throw ex;
                }
            }
            return _connection;
        }
    }
}