using EventHorizon.Zone.System.Agent.Behavior.Api;
using EventHorizon.Zone.System.Agent.Behavior.Interpreter;
using EventHorizon.Zone.System.Agent.Behavior.Interpreters;
using EventHorizon.Zone.System.Agent.Behavior.Load;
using EventHorizon.Zone.System.Agent.Behavior.State;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace EventHorizon.Game.Server.Zone.Core
{
    public static class SystemAgentBehaviorExtensions
    {
        public static void AddSystemAgentBehavior(this IServiceCollection services)
        {
            services
                .AddSingleton<AgentBehaviorScriptRepository, InMemoryAgentBehaviorScriptRepository>()
                .AddSingleton<ActionBehaviorInterpreter, ActionInterpreter>()
                .AddSingleton<BehaviorInterpreterKernel, BehaviorInterpreterDoWhileKernel>()
                .AddSingleton<BehaviorInterpreterMap, BehaviorInterpreterInMemoryMap>();
        }
        public static void UseSystemAgentBehavior(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetService<IMediator>()
                    .Send(
                        new LoadAgentBehaviorSystemEvent()
                    ).GetAwaiter(
                    ).GetResult();
            }
        }
    }
}