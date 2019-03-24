using System.Reflection;
using EventHorizon.Game.Server.Zone.Admin.SystemWatcher;
using EventHorizon.Game.Server.Zone.Admin.SystemWatcher.State;
using EventHorizon.Game.Server.Zone.Admin.SystemWatcher.Timer;
using EventHorizon.Game.Server.Zone.Core.Connection;
using EventHorizon.Game.Server.Zone.Core.Connection.Impl;
using EventHorizon.Game.Server.Zone.Core.DirectoryService;
using EventHorizon.Game.Server.Zone.Core.IdPool;
using EventHorizon.Game.Server.Zone.Core.IdPool.Impl;
using EventHorizon.Game.Server.Zone.Core.Info;
using EventHorizon.Game.Server.Zone.Core.Json;
using EventHorizon.Game.Server.Zone.Core.Model;
using EventHorizon.Game.Server.Zone.Core.RandomNumber;
using EventHorizon.Game.Server.Zone.Core.RandomNumber.Impl;
using EventHorizon.Game.Server.Zone.Core.Register;
using EventHorizon.Game.Server.Zone.Core.ServerProperty;
using EventHorizon.Game.Server.Zone.Core.ServerProperty.Impl;
using EventHorizon.Game.Server.Zone.External.DateTimeService;
using EventHorizon.Game.Server.Zone.External.DirectoryService;
using EventHorizon.Game.Server.Zone.External.Info;
using EventHorizon.Game.Server.Zone.External.Json;
using EventHorizon.Game.Server.Zone.External.RandomNumber;
using EventHorizon.TimerService;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
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
                serviceScope.ServiceProvider.GetService<IMediator>().Publish(new StartWatchingSystemEvent()).GetAwaiter().GetResult();
            }
            return app;
        }
    }
}