using EventHorizon.Game.Server.Zone.Load.Settings.Events;
using EventHorizon.Game.Server.Zone.Load.Settings.Factory;
using EventHorizon.Game.Server.Zone.Settings.Load;
using EventHorizon.Zone.Core.Model.Settings;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace EventHorizon.Game.Server.Zone.Core
{
    public static class LoadExtensions
    {
        public static IServiceCollection AddServerLoad(
            this IServiceCollection services
        )
        {
            var zoneSettingsFactory = new ZoneSettingsFactory();
            return services
                .AddSingleton<IZoneSettingsFactory>(
                    zoneSettingsFactory
                ).AddSingleton<IZoneSettingsSetter>(
                    zoneSettingsFactory
                ).AddTransient<ZoneSettings>(
                    _ => zoneSettingsFactory.Settings
                )
            ;
        }
        public static IApplicationBuilder UseServerLoad(
            this IApplicationBuilder app
        )
        {
            using (var serviceScope = app.CreateServiceScope())
            {
                serviceScope.ServiceProvider
                    .GetService<IMediator>()
                    .Publish(
                        new LoadZoneSettingsEvent()
                    ).GetAwaiter().GetResult();
            }
            return app;
        }
    }
}