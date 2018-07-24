using System;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Core.Model;
using EventHorizon.Identity;
using MediatR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EventHorizon.Game.Server.Zone.Core.Connection.Impl
{
    public class CoreConnectionFactory : ICoreConnectionFactory
    {
        readonly ILogger _logger;
        readonly ILoggerFactory _loggerFactory;
        readonly IMediator _mediator;
        readonly CoreSettings _coreSettings;
        readonly ICoreConnectionCache _connectionCache;

        public CoreConnectionFactory(ILoggerFactory loggerFactory,
            IMediator mediator,
            IOptions<CoreSettings> playerSettings,
            ICoreConnectionCache connectionCache)
        {
            _logger = loggerFactory.CreateLogger<CoreConnectionFactory>();

            _loggerFactory = loggerFactory;
            _mediator = mediator;
            _coreSettings = playerSettings.Value;
            _connectionCache = connectionCache;
        }

        public async Task<ICoreConnection> GetConnection()
        {
            return new CoreConnection(
                _loggerFactory.CreateLogger<CoreConnection>(),
                await _connectionCache.GetConnection($"{_coreSettings.Server}/zoneCore", options =>
                    {
                        options.AccessTokenProvider = async () => await _mediator.Send(new RequestIdentityAccessTokenEvent());
                    }));
        }
    }
}