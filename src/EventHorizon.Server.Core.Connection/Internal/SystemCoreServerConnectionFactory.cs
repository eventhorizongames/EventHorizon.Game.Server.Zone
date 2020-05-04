namespace EventHorizon.Server.Core.Connection.Internal
{
    using System;
    using System.Threading.Tasks;
    using EventHorizon.Identity.AccessToken;
    using EventHorizon.Server.Core.Connection.Disconnected;
    using EventHorizon.Server.Core.Connection.Model;
    using MediatR;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public class SystemCoreServerConnectionFactory : CoreServerConnectionFactory
    {
        private readonly ILogger _logger;
        private readonly ILoggerFactory _loggerFactory;
        private readonly IMediator _mediator;
        private readonly CoreSettings _coreSettings;
        private readonly CoreServerConnectionCache _connectionCache;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public SystemCoreServerConnectionFactory(
            ILoggerFactory loggerFactory,
            IMediator mediator,
            IOptions<CoreSettings> playerSettings,
            CoreServerConnectionCache connectionCache,
            IServiceScopeFactory serviceScopeFactory
        )
        {
            _logger = loggerFactory.CreateLogger<SystemCoreServerConnectionFactory>();

            _loggerFactory = loggerFactory;
            _mediator = mediator;
            _coreSettings = playerSettings.Value;
            _connectionCache = connectionCache;
            _serviceScopeFactory = serviceScopeFactory;
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
                    },
                    this.OnClose
                )
            );
        }

        private Task OnClose(
            Exception exception
        )
        {
            using (var serviceScope = _serviceScopeFactory.CreateScope())
            {
                return serviceScope.ServiceProvider.GetService<IMediator>().Publish(
                    new ServerCoreConnectionDisconnected()
                );
            }
        }
    }
}