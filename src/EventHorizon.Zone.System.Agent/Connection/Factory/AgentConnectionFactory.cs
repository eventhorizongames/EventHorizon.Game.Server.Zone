namespace EventHorizon.Zone.System.Agent.Connection.Factory;

using EventHorizon.Identity.AccessToken;
using EventHorizon.Zone.System.Agent.Model;

using global::System.Threading.Tasks;

using MediatR;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

public class AgentConnectionFactory
    : IAgentConnectionFactory
{
    private readonly ILoggerFactory _loggerFactory;
    private readonly IMediator _mediator;
    private readonly AgentSettings _agentSettings;
    private readonly IAgentConnectionCache _connectionCache;

    public AgentConnectionFactory(
        ILoggerFactory loggerFactory,
        IMediator mediator,
        IOptions<AgentSettings> agentSettings,
        IAgentConnectionCache connectionCache
    )
    {
        _loggerFactory = loggerFactory;
        _mediator = mediator;
        _agentSettings = agentSettings.Value;
        _connectionCache = connectionCache;
    }

    public async Task<IAgentConnection> GetConnection()
    {
        return new AgentConnection(
            _loggerFactory.CreateLogger<AgentConnection>(),
            await _connectionCache.GetConnection(
                $"{_agentSettings.Server}/agent",
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
