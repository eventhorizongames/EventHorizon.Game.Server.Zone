
using EventHorizon.Game.Server.Zone.Agent.Move;
using EventHorizon.Game.Server.Zone.Agent.Move.Impl;
using EventHorizon.Game.Server.Zone.Agent.Move.Repository;
using EventHorizon.Game.Server.Zone.Agent.Move.Repository.Impl;
using EventHorizon.Game.Server.Zone.Agent.Startup;
using EventHorizon.Game.Server.Zone.Agent.State.Impl;
using EventHorizon.Game.Server.Zone.State.Repository;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventHorizon.Game.Server.Zone.Agent
{
    public static class AgentExtensions
    {
        public static void AddAgent(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IAgentRepository, AgentRepository>();
            services.AddSingleton<IMoveAgentRepository, MoveAgentRepository>();
            services.AddSingleton<IMoveRegisteredAgentsTimer, MoveRegisteredAgentsTimer>();
        }
        public static void UseAgent(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var mediator = serviceScope.ServiceProvider.GetService<IMediator>();
                mediator.Send(new LoadZoneAgentStateEvent()).GetAwaiter().GetResult();
                mediator.Publish(new StartMoveRegisteredAgentsTimerEvent()).GetAwaiter().GetResult();
            }
        }
    }
}