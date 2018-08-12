using EventHorizon.Game.Server.Zone.Agent.Startup;
using EventHorizon.Game.Server.Zone.Core.IdPool;
using EventHorizon.Game.Server.Zone.Core.IdPool.Impl;
using EventHorizon.Game.Server.Zone.Core.Model;
using EventHorizon.Game.Server.Zone.Core.Register;
using EventHorizon.Game.Server.Zone.Core.ServerProperty;
using EventHorizon.Game.Server.Zone.Core.ServerProperty.Impl;
using EventHorizon.Game.Server.Zone.Load;
using EventHorizon.Game.Server.Zone.Load.Events.Settings;
using EventHorizon.Game.Server.Zone.Load.Factory;
using EventHorizon.Game.Server.Zone.Load.JSON;
using EventHorizon.Game.Server.Zone.Load.Model;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventHorizon.Game.Server.Zone.Core
{
    public static class LoadExtensions
    {
        public static void AddLoad(this IServiceCollection services, IConfiguration configuration)
        {
            var zoneSettingsFactory = new ZoneSettingsFactory();
            services.AddSingleton<IZoneSettingsFactory>(zoneSettingsFactory);
            services.AddSingleton<IZoneSettingsSetter>(zoneSettingsFactory);
            services.AddTransient<IJsonFileLoader, JsonFileLoader>();
        }
        public static void UseLoad(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var mediator = serviceScope.ServiceProvider.GetService<IMediator>();
                mediator.Publish(new LoadZoneSettingsEvent()).GetAwaiter().GetResult();
            }
        }
    }
}