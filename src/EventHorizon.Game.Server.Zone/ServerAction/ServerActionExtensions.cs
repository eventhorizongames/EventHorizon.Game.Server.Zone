using System.Collections.Generic;
using System.Linq;
using EventHorizon.Game.Server.Zone.ServerAction.State;
using EventHorizon.Game.Server.Zone.ServerAction.State.Impl;
using EventHorizon.Game.Server.Zone.ServerAction.Timer;
using EventHorizon.TimerService;
using Microsoft.Extensions.DependencyInjection;

namespace EventHorizon.Game.Server.Zone.ServerAction
{
    public static class ServerActionExtensions
    {
        public static void AddServerAction(this IServiceCollection services)
        {
            services
                .AddSingleton<IServerActionQueue, ServerActionQueue>()
                .AddSingleton<ITimerTask, RunServerActionsTimerTask>();
        }
    }
}