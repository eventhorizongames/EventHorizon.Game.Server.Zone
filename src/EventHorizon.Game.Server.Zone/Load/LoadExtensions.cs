using EventHorizon.Zone.System.Agent.Startup;
using EventHorizon.Game.Server.Zone.Core.IdPool;
using EventHorizon.Game.Server.Zone.Core.IdPool.Impl;
using EventHorizon.Game.Server.Zone.Core.Model;
using EventHorizon.Game.Server.Zone.Core.Register;
using EventHorizon.Game.Server.Zone.Core.ServerProperty;
using EventHorizon.Game.Server.Zone.Core.ServerProperty.Impl;
using EventHorizon.Game.Server.Zone.Load;
using EventHorizon.Game.Server.Zone.Load.Map;
using EventHorizon.Game.Server.Zone.Load.Map.Events;
using EventHorizon.Game.Server.Zone.Load.Map.Factory;
using EventHorizon.Game.Server.Zone.Load.Map.Model;
using EventHorizon.Game.Server.Zone.Load.Settings.Events;
using EventHorizon.Game.Server.Zone.Load.Settings.Factory;
using EventHorizon.Game.Server.Zone.Load.Settings.Model;
using EventHorizon.Game.Server.Zone.Settings.Load;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventHorizon.Game.Server.Zone.Core
{
    public static class LoadExtensions
    {
        public static IServiceCollection AddLoad(this IServiceCollection services, IConfiguration configuration)
        {
            var zoneSettingsFactory = new ZoneSettingsFactory();
            var zoneMapFactory = new ZoneMapFactory();
            return services.AddSingleton<IZoneSettingsFactory>(
                zoneSettingsFactory
            ).AddSingleton<IZoneSettingsSetter>(
                zoneSettingsFactory
            ).AddTransient<ZoneSettings>(
                _ => zoneSettingsFactory.Settings
            ).AddSingleton<IZoneMapFactory>(
                zoneMapFactory
            ).AddSingleton<IZoneMapSetter>(
                zoneMapFactory
            ).AddTransient<ZoneMap>(
                _ => zoneMapFactory.Map
            );
        }
        public static void UseLoad(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var mediator = serviceScope.ServiceProvider.GetService<IMediator>();
                mediator.Publish(
                    new LoadZoneSettingsEvent()
                ).GetAwaiter().GetResult();
                mediator.Publish(
                    new LoadZoneMapEvent()
                ).GetAwaiter().GetResult();
            }
        }
    }
}