namespace EventHorizon.Zone.System.Agent;

using global::System.Collections.Generic;
using global::System.Threading.Tasks;

using EventHorizon.Zone.System.Agent.Connection;
using EventHorizon.Zone.System.Agent.Connection.Factory;
using EventHorizon.Zone.System.Agent.Connection.Model;

using Microsoft.Extensions.DependencyInjection;

public static class AgentConnectionExtensions
{
    public static Task<IList<AgentDetails>> GetAgentList(
        this IAgentConnection connection,
        string tag
    )
    {
        return connection.SendAction<IList<AgentDetails>>(
            "GetAgentsByZoneTag",
            tag
        );
    }

    public static Task UpdateAgent(
        this IAgentConnection connection,
        AgentDetails agent
    )
    {
        return connection.SendAction(
            "UpdateAgent",
            agent
        );
    }

    public static IServiceCollection AddAgentConnection(
        this IServiceCollection services
    )
    {
        return services
            .AddSingleton<IAgentConnectionCache, AgentConnectionCache>()
            .AddTransient<IAgentConnectionFactory, AgentConnectionFactory>()
            .AddTransient<IAgentConnection>(
                (factoryServices) => factoryServices
                    .GetRequiredService<IAgentConnectionFactory>()
                    .GetConnection()
                    .GetAwaiter().GetResult()
            );
    }
}
