using System.Collections.Generic;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Agent.Connection;
using EventHorizon.Game.Server.Zone.Agent.Connection.Factory;
using EventHorizon.Game.Server.Zone.Agent.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventHorizon.Game.Server.Zone.Agent
{
    public static class AgentConnectionExtensions
    {
        public static Task<IList<AgentDetails>> GetAgentList(this IAgentConnection connection, string tag)
        {
            return connection.SendAction<IList<AgentDetails>>("GetAgentsByZoneTag", tag);
        }
        public static Task UpdateAgent(this IAgentConnection connection, AgentDetails agent)
        {
            return connection.SendAction("UpdateAgent", agent);
        }
        public static IServiceCollection AddAgentConnection(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddSingleton<IAgentConnectionCache, AgentConnectionCache>()
                .AddTransient<IAgentConnectionFactory, AgentConnectionFactory>()
                .AddTransient<IAgentConnection>((factoryServices) => factoryServices.GetRequiredService<IAgentConnectionFactory>().GetConnection().GetAwaiter().GetResult());
        }
    }
}