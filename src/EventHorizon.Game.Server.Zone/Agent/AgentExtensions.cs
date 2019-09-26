using EventHorizon.Zone.System.Agent.Model;
using EventHorizon.Zone.System.Agent.Move.Impl;
using EventHorizon.Zone.System.Agent.Move.Repository.Impl;
using EventHorizon.Zone.System.Agent.Save;
using EventHorizon.Zone.System.Agent.Startup;
using EventHorizon.Zone.System.Agent.State.Impl;
using EventHorizon.Schedule;
using EventHorizon.TimerService;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using EventHorizon.Zone.System.Agent.Model.State;

namespace EventHorizon.Zone.System.Agent
{
    public static class AgentExtensions
    {
        public static void AddAgent(
            this IServiceCollection services, 
            IConfiguration configuration
        )
        {
            services
                .Configure<AgentSettings>(configuration.GetSection("Agent"))
                .AddTransient<IAgentRepository, AgentRepository>()
                .AddTransient<IMoveAgentRepository, MoveAgentRepository>()
                .AddSingleton<ITimerTask, MoveRegisteredAgentsTimer>()
                .AddSingleton<IScheduledTask, SaveAgentStateScheduledTask>()
                .AddAgentConnection(configuration);
        }
        public static void UseAgent(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var mediator = serviceScope.ServiceProvider.GetService<IMediator>();
                mediator.Send(new LoadZoneAgentStateEvent()).GetAwaiter().GetResult();
            }
        }
    }
}