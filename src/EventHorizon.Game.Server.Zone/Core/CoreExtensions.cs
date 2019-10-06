using EventHorizon.Game.Server.Zone.Core.Connection;
using EventHorizon.Game.Server.Zone.Core.Connection.Impl;
using EventHorizon.Game.Server.Zone.Core.DirectoryService;
using EventHorizon.Game.Server.Zone.Core.Id;
using EventHorizon.Game.Server.Zone.Core.Info;
using EventHorizon.Game.Server.Zone.Core.Json;
using EventHorizon.Game.Server.Zone.Core.Model;
using EventHorizon.Game.Server.Zone.Core.RandomNumber.Impl;
using EventHorizon.Game.Server.Zone.Core.Register;
using EventHorizon.Game.Server.Zone.Core.ServerProperty;
using EventHorizon.Game.Server.Zone.Core.ServerProperty.Impl;
using EventHorizon.Zone.Core.Model.DateTimeService;
using EventHorizon.Zone.Core.Model.DirectoryService;
using EventHorizon.Zone.Core.Model.Id;
using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.Core.Model.Json;
using EventHorizon.Zone.Core.Model.RandomNumber;
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
            services
                .AddTransient<DirectoryResolver, ServerDirectoryResolver>()
                .AddTransient<IJsonFileLoader, JsonFileLoader>()
                .AddTransient<IJsonFileSaver, JsonFileSaver>()
                .AddSingleton<IdPool, InMemoryStaticIdPool>()
                .AddSingleton<IRandomNumberGenerator, RandomNumberGenerator>()
                .AddSingleton<IServerProperty, ServerPropertyImpl>()
                .AddSingleton<ServerInfo, ZoneServerInfo>()
                .Configure<AuthSettings>(configuration.GetSection("Auth"))
                .Configure<CoreSettings>(configuration.GetSection("Core"))
                .AddSingleton<ICoreConnectionCache, CoreConnectionCache>()
                .AddSingleton<IDateTimeService, DateTimeService.DateTimeService>()
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