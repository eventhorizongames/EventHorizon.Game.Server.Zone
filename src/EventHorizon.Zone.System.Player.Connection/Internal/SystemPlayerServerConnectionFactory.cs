namespace EventHorizon.Zone.System.Player.Connection.Internal;

using EventHorizon.Identity.AccessToken;
using EventHorizon.Zone.System.Player.Connection.Model;

using global::System.Threading.Tasks;

using MediatR;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

public class SystemPlayerServerConnectionFactory
    : PlayerServerConnectionFactory
{
    private readonly ILoggerFactory _loggerFactory;
    private readonly IMediator _mediator;
    private readonly PlayerServerConnectionSettings _connectionSettings;
    private readonly PlayerServerConnectionCache _connectionCache;

    public SystemPlayerServerConnectionFactory(
        ILoggerFactory loggerFactory,
        IMediator mediator,
        IOptions<PlayerServerConnectionSettings> connectionSettings,
        PlayerServerConnectionCache connectionCache
    )
    {
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
