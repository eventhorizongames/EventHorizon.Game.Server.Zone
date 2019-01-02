using System;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Core.Player.Model;
using EventHorizon.Game.Server.Zone.Agent.Connection;
using MediatR;
using Microsoft.AspNetCore.Http.Connections.Client;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EventHorizon.Game.Server.Zone.Agent.Connection.Factory
{
    public class AgentConnectionCache : IAgentConnectionCache, IDisposable
    {
        readonly ILogger _logger;

        private HubConnection _connection;

        public AgentConnectionCache(ILogger<AgentConnectionCache> logger)
        {
            _logger = logger;
        }

        public void Dispose()
        {
            _connection?.DisposeAsync().GetAwaiter().GetResult();
        }

        public async Task<HubConnection> GetConnection(string url, Action<HttpConnectionOptions> configureHttpConnection)
        {
            if (_connection == null)
            {
                try
                {
                    _connection = new HubConnectionBuilder()
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