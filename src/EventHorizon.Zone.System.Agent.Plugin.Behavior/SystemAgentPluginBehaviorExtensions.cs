namespace EventHorizon.Game.Server.Zone;

using EventHorizon.TimerService;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Api;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Interpreter;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Interpreters;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Load;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.State;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.State.Queue;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Timer;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

public static class SystemAgentPluginBehaviorExtensions
{
    public static IServiceCollection AddSystemAgentPluginBehavior(
        this IServiceCollection services
    ) => services
        .AddSingleton<ITimerTask, RunPendingActorBehaviorTicksTimer>()

        .AddSingleton<ActorBehaviorTreeRepository, InMemoryActorBehaviorTreeRepository>()
        .AddSingleton<ActorBehaviorTickQueue, InMemoryActorBehaviorTickQueue>()

        .AddSingleton<BehaviorInterpreterMap, BehaviorInterpreterInMemoryMap>()
        .AddSingleton<BehaviorInterpreterKernel, BehaviorInterpreterDoWhileKernel>()

        .AddSingleton<ActionBehaviorInterpreter, ActionInterpreter>()
        .AddSingleton<ConditionBehaviorInterpreter, ConditionInterpreter>()
    ;

    public static IApplicationBuilder UseSystemAgentPluginBehavior(
        this IApplicationBuilder app
    ) => app.SendMediatorCommand(
        new LoadAgentBehaviorSystem()
    );
}
