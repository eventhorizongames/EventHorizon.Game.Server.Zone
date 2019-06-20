using EventHorizon.TimerService;
using EventHorizon.Zone.System.Agent.Behavior.Api;
using EventHorizon.Zone.System.Agent.Behavior.Interpreter;
using EventHorizon.Zone.System.Agent.Behavior.Interpreters;
using EventHorizon.Zone.System.Agent.Behavior.Load;
using EventHorizon.Zone.System.Agent.Behavior.State;
using EventHorizon.Zone.System.Agent.Behavior.Timer;
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
                .AddSingleton<ITimerTask, BehaviorTreeUpdateTriggerTimerTask>()

                .AddSingleton<ActorBehaviorScriptRepository, InMemoryActorBehaviorScriptRepository>()
                .AddSingleton<ActorBehaviorTreeRepository, InMemoryActorBehaviorTreeRepository>()

                .AddSingleton<BehaviorInterpreterMap, BehaviorInterpreterInMemoryMap>()
                .AddSingleton<BehaviorInterpreterKernel, BehaviorInterpreterDoWhileKernel>()

                .AddSingleton<ActionBehaviorInterpreter, ActionInterpreter>();

        }
        public static void UseSystemAgentBehavior(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetService<IMediator>()
                    .Send(
                        new LoadAgentBehaviorSystem()
                    ).GetAwaiter(
                    ).GetResult();
            }
        }
    }
}