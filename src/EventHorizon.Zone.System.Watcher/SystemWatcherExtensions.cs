using EventHorizon.TimerService;
using EventHorizon.Zone.System.Watcher.State;
using EventHorizon.Zone.System.Watcher.Timer;

using MediatR;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace EventHorizon.Game.Server.Zone
{
    public static class SystemWatcherExtensions
    {
        public static IServiceCollection AddSystemWatcher(
            this IServiceCollection services
        ) => services
                .AddSingleton<FileSystemWatcherState, InMemoryFileSystemWatcherState>()
                .AddSingleton<PendingReloadState, InMemoryPendingReloadState>()
                .AddTransient<ITimerTask, WatchForSystemReloadTimer>()
            ;

        public static IApplicationBuilder UseSystemWatcher(
            this IApplicationBuilder app
        )
        {
            using (var serviceScope = app.CreateServiceScope())
            {
                return app;
            }
        }
    }
}
