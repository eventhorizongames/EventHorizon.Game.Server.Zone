using EventHorizon.Plugin.Zone.System.Combat.Events;
using EventHorizon.Plugin.Zone.System.Combat.Life;
using EventHorizon.Plugin.Zone.System.Combat.Model.Life;
using EventHorizon.Plugin.Zone.System.Combat.State;
using EventHorizon.Plugin.Zone.System.Combat.Timer;
using EventHorizon.TimerService;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventHorizon.Game.Server.Zone.Core
{
    public static class SystemCombatExtensions
    {
        public static void AddSystemCombat(this IServiceCollection services)
        {
            services
            .AddSingleton<IEntityQueue<UpdateEntityLife>, EntityQueue<UpdateEntityLife>>()
            .AddTransient<ITimerTask, UpdateEntityLifeTimer>()
            // .AddTransient<ITimerTask, UpdateEntityLevelTimer>()
            .AddSingleton<ILifeStateChange, LifeStateChange>();
        }
        public static void UseSystemCombat(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetService<IMediator>().Publish(new SetupCombatSystemGuiEvent()).GetAwaiter().GetResult();
            }
        }
    }
}