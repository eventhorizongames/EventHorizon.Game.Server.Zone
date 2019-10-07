using System;
using System.Threading.Tasks;
using EventHorizon.Identity.AccessToken;
using EventHorizon.Server.Core.External.Model;
using MediatR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EventHorizon.Server.Core.External.Connection.Internal
{
    public class SystemCoreServerConnectionFactory : CoreServerConnectionFactory
    {
        readonly ILogger _logger;
        readonly ILoggerFactory _loggerFactory;
        readonly IMediator _mediator;
        readonly CoreSettings _coreSettings;
        readonly CoreServerConnectionCache _connectionCache;

        public SystemCoreServerConnectionFactory(
            ILoggerFactory loggerFactory,
            IMediator mediator,
            IOptions<CoreSettings> playerSettings,
            CoreServerConnectionCache connectionCache
        )
        {
            _logger = loggerFactory.CreateLogger<SystemCoreServerConnectionFactory>();

            _loggerFactory = loggerFactory;
            _mediator = mediator;
            _coreSettings = playerSettings.Value;
            _connectionCache = connectionCache;
        }

        public async Task<CoreServerConnection> GetConnection()
        {
            return new SystemCoreServerConnection(
                _loggerFactory.CreateLogger<SystemCoreServerConnection>(),
                await _connectionCache.GetConnection(
                    $"{_coreSettings.Server}/zoneCore", 
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