namespace EventHorizon.Game.Server.Zone.Core
{
    using EventHorizon.Game.Server.Zone.Load.Settings.Events;
    using EventHorizon.Game.Server.Zone.Load.Settings.Factory;
    using EventHorizon.Game.Server.Zone.Settings.Load;

    using MediatR;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

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
                ).AddTransient(
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
                    .GetRequiredService<IMediator>()
                    .Publish(
                        new LoadZoneSettingsEvent()
                    ).GetAwaiter().GetResult();
            }
            return app;
        }
    }
}
