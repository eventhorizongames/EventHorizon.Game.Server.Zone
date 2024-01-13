namespace EventHorizon.Game.Server.Zone;

using EventHorizon.TimerService;
using EventHorizon.Zone.System.Agent.Model.State;
using EventHorizon.Zone.System.Agent.Move.Repository;
using EventHorizon.Zone.System.Agent.Move.Timer;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

public static class SystemAgentPluginMoveExtensions
{
    public static IServiceCollection AddSystemAgentPluginMove(
        this IServiceCollection services
    )
    {
        return services
            .AddSingleton<IMoveAgentRepository, MoveAgentRepository>()
            .AddSingleton<ITimerTask, MoveRegisteredAgentsTimer>()
        ;
    }
    public static IApplicationBuilder UseSystemAgentPluginMove(
        this IApplicationBuilder app
    )
    {
        return app;
    }
}
