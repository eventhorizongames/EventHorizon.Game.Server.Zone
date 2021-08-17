namespace EventHorizon.Game.Server.Zone
{
    using EventHorizon.TimerService;
    using EventHorizon.Zone.System.Agent;
    using EventHorizon.Zone.System.Agent.Events.Startup;
    using EventHorizon.Zone.System.Agent.Model;
    using EventHorizon.Zone.System.Agent.Model.State;
    using EventHorizon.Zone.System.Agent.Plugin.Behavior.Timer;
    using EventHorizon.Zone.System.Agent.Save;
    using EventHorizon.Zone.System.Agent.State;

    using MediatR;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class SystemAgentExtensions
    {
        public static IServiceCollection AddSystemAgent(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            return services
                .Configure<AgentSettings>(
                    configuration.GetSection("Agent")
                )
                .AddTransient<IAgentRepository, AgentWrappedEntityRepository>()
                .AddSingleton<ITimerTask, SaveAgentStateTimerTask>()
                .AddSingleton<ITimerTask, CheckForStaleAgentPathTimer>()
                .AddAgentConnection();
        }
        public static IApplicationBuilder UseSystemAgent(
            this IApplicationBuilder app
        )
        {
            app.SendMediatorCommand(
                new LoadZoneAgentStateEvent()
            );

            return app;
        }
    }
}
