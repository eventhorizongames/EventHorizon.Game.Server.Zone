using System.Threading.Tasks;
using EventHorizon.Identity.AccessToken;
using EventHorizon.Zone.System.Agent.Model;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EventHorizon.Zone.System.Agent.Connection.Factory
{
    public class AgentConnectionFactory : IAgentConnectionFactory
    {
        readonly ILogger _logger;
        readonly ILoggerFactory _loggerFactory;
        readonly IMediator _mediator;
        readonly AgentSettings _agentSettings;
        readonly IAgentConnectionCache _connectionCache;

        public AgentConnectionFactory(
            ILoggerFactory loggerFactory,
            IMediator mediator,
            IOptions<AgentSettings> agentSettings,
            IAgentConnectionCache connectionCache
        )
        {
            _logger = loggerFactory.CreateLogger<AgentConnectionFactory>();

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
}