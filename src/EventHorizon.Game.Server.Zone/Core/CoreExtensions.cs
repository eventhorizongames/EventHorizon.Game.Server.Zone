using System.Reflection;
using EventHorizon.Game.Server.Zone.Core.Connection;
using EventHorizon.Game.Server.Zone.Core.Connection.Impl;
using EventHorizon.Game.Server.Zone.Core.IdPool;
using EventHorizon.Game.Server.Zone.Core.IdPool.Impl;
using EventHorizon.Game.Server.Zone.Core.Json;
using EventHorizon.Game.Server.Zone.Core.Json.Impl;
using EventHorizon.Game.Server.Zone.Core.Model;
using EventHorizon.Game.Server.Zone.Core.RandomNumber;
using EventHorizon.Game.Server.Zone.Core.RandomNumber.Impl;
using EventHorizon.Game.Server.Zone.Core.Register;
using EventHorizon.Game.Server.Zone.Core.ServerProperty;
using EventHorizon.Game.Server.Zone.Core.ServerProperty.Impl;
using EventHorizon.Game.Server.Zone.External.Info;
using EventHorizon.Game.Server.Zone.External.Json;
using EventHorizon.Game.Server.Zone.External.RandomNumber;
using EventHorizon.Game.Server.Zone.Core.Info;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventHorizon.Game.Server.Zone.Core
{
    public static class CoreExtensions
    {
        public static void AddZoneCore(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(typeof(Agent.PluginStartup), typeof(Agent.Ai.PluginStartup))
                .AddTransient<IJsonFileLoader, JsonFileLoader>()
                .AddTransient<IJsonFileSaver, JsonFileSaver>()
                .AddSingleton<IIdPool, IdPoolImpl>()
                .AddSingleton<IRandomNumberGenerator, RandomNumberGenerator>()
                .AddSingleton<IServerProperty, ServerPropertyImpl>()
                .AddSingleton<ServerInfo, ZoneServerInfo>()
                .Configure<AuthSettings>(configuration.GetSection("Auth"))
                .Configure<CoreSettings>(configuration.GetSection("Core"))
                .AddSingleton<ICoreConnectionCache, CoreConnectionCache>()
                .AddTransient<ICoreConnectionFactory, CoreConnectionFactory>();
        }
        public static void UseZoneCore(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetService<IMediator>().Publish(new FillServerPropertiesEvent()).GetAwaiter().GetResult();
                serviceScope.ServiceProvider.GetService<IMediator>().Publish(new RegisterWithCoreServerEvent()).GetAwaiter().GetResult();
            }
        }
    }
}