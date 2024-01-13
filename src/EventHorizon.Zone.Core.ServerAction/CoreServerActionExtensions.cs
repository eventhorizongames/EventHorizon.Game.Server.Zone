namespace EventHorizon.Game.Server.Zone;

using EventHorizon.TimerService;
using EventHorizon.Zone.Core.ServerAction.State;
using EventHorizon.Zone.Core.ServerAction.Timer;

using Microsoft.Extensions.DependencyInjection;

public static class CoreServerActionExtensions
{
    public static IServiceCollection AddCoreServerAction(
        this IServiceCollection services
    ) => services
        .AddSingleton<IServerActionQueue, ServerActionQueue>()
        .AddSingleton<ITimerTask, RunServerActionsTimerTask>()
    ;
}
