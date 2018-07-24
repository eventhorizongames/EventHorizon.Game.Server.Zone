using EventHorizon.Game.Server.Zone.Agent.Move;
using EventHorizon.Game.Server.Zone.Agent.Move.Impl;
using EventHorizon.Game.Server.Zone.Agent.Move.Repository;
using EventHorizon.Game.Server.Zone.Agent.Move.Repository.Impl;
using EventHorizon.Game.Server.Zone.Agent.Startup;
using EventHorizon.Game.Server.Zone.Core.IdPool;
using EventHorizon.Game.Server.Zone.Core.IdPool.Impl;
using EventHorizon.Game.Server.Zone.Core.Model;
using EventHorizon.Game.Server.Zone.Core.Register;
using EventHorizon.Game.Server.Zone.Core.ServerProperty;
using EventHorizon.Game.Server.Zone.Core.ServerProperty.Impl;
using EventHorizon.Game.Server.Zone.Load;
using EventHorizon.Game.Server.Zone.Load.Events.Settings;
using EventHorizon.Game.Server.Zone.Load.Factory;
using EventHorizon.Game.Server.Zone.Load.Model;
using EventHorizon.Schedule;
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