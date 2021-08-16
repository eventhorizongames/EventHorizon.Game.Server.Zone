using System;
using System.Threading.Tasks;

using EventHorizon.Identity.AccessToken;
using EventHorizon.Zone.System.Player.Connection.Model;

using MediatR;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EventHorizon.Zone.System.Player.Connection.Internal
{
    public class SystemPlayerServerConnectionFactory : PlayerServerConnectionFactory
    {
        readonly ILogger _logger;
        readonly ILoggerFactory _loggerFactory;
        readonly IMediator _mediator;
        readonly PlayerServerConnectionSettings _connectionSettings;
        readonly PlayerServerConnectionCache _connectionCache;

        public SystemPlayerServerConnectionFactory(
            ILoggerFactory loggerFactory,
            IMediator mediator,
            IOptions<PlayerServerConnectionSettings> connectionSettings,
            PlayerServerConnectionCache connectionCache
        )
        {
            _logger = loggerFactory.CreateLogger<SystemPlayerServerConnectionFactory>();

            _loggerFactory = loggerFactory;
            _mediator = mediator;
            _connectionSettings = connectionSettings.Value;
            _connectionCache = connectionCache;
        }

        public async Task<PlayerServerConnection> GetConnection()
        {
            return new SystemPlayerServerConnection(
                _loggerFactory.CreateLogger<SystemPlayerServerConnection>(),
                await _connectionCache.GetConnection(
                    $"{_connectionSettings.Server}/playerBus",
                    options =>
                    {
                        options.AccessTokenProvider = async () =>
                            await _mediator.Send(
                                new RequestIdentityAccessTokenEvent()
                            );
                    }
                )
            );
        }
    }
}
