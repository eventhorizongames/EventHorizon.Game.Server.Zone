using EventHorizon.Zone.System.Agent.Model;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using EventHorizon.Zone.System.Agent;
using EventHorizon.Zone.System.Agent.Events.Startup;
using EventHorizon.Zone.System.Agent.Model.State;
using EventHorizon.Schedule;
using EventHorizon.Zone.System.Agent.Save;
using EventHorizon.Zone.System.Agent.State;

namespace EventHorizon.Game.Server.Zone
{
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
                .AddSingleton<IScheduledTask, SaveAgentStateScheduledTask>()
                .AddAgentConnection();
        }
        public static IApplicationBuilder UseSystemAgent(
            this IApplicationBuilder app
        )
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var mediator = serviceScope.ServiceProvider.GetService<IMediator>();
                mediator.Send(
                    new LoadZoneAgentStateEvent()
                ).GetAwaiter().GetResult();

                return app;
            }
        }
    }
}