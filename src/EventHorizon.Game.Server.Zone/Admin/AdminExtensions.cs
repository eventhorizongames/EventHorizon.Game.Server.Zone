using EventHorizon.Game.Server.Zone.Admin.SystemWatcher;
using EventHorizon.Game.Server.Zone.Admin.SystemWatcher.State;
using EventHorizon.Game.Server.Zone.Admin.SystemWatcher.Timer;
using EventHorizon.TimerService;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace EventHorizon.Game.Server.Zone.Core
{
    public static class AdminExtensions
    {
        public static IServiceCollection AddZoneAdmin(this IServiceCollection services)
        {
            return services
                .AddSingleton<ISystemWatcherState, SystemWatcherState>()
                .AddTransient<ITimerTask, WatcherReloadSystemTimer>()
            ;
        }
        public static IApplicationBuilder UseZoneAdmin(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider
                    .GetService<IMediator>()
                    .Publish(new StartWatchingSystemEvent())
                    .GetAwaiter().GetResult();
            }
            return app;
        }
    }
}