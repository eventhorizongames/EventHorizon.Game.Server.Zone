using System;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Core.Player.Model;
using MediatR;
using Microsoft.AspNetCore.Http.Connections.Client;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EventHorizon.Game.Server.Core.Player.Connection.Impl
{
    public class ConnectionCache : IConnectionCache, IDisposable
    {
        readonly ILogger _logger;

        private HubConnection _connection;

        public ConnectionCache(ILogger<ConnectionCache> logger)
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
                    _logger.LogError(ex, "Error connecting");
                    _connection = null;
                    throw ex;
                }
            }
            return _connection;
        }
    }
}