using System.Reflection;
using EventHorizon.Zone.Core.DateTimeService;
using EventHorizon.Zone.Core.Events.Lifetime;
using EventHorizon.Zone.Core.Id;
using EventHorizon.Zone.Core.Info;
using EventHorizon.Zone.Core.Json;
using EventHorizon.Zone.Core.Lifetime.State;
using EventHorizon.Zone.Core.Model.DateTimeService;
using EventHorizon.Zone.Core.Model.DirectoryService;
using EventHorizon.Zone.Core.Model.FileService;
using EventHorizon.Zone.Core.Model.Id;
using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.Core.Model.Json;
using EventHorizon.Zone.Core.Model.RandomNumber;
using EventHorizon.Zone.Core.Model.ServerProperty;
using EventHorizon.Zone.Core.Plugin.LocalFileSystem;
using EventHorizon.Zone.Core.RandomNumber;
using EventHorizon.Zone.Core.ServerProperty;
using EventHorizon.Zone.Core.ServerProperty.Fill;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EventHorizon.Game.Server.Zone
{
    public static class CoreExtensions
    {
        public static IServiceCollection AddCore(
            this IServiceCollection services,
            Assembly[] systemProvidedAssemblyList
        ) => services
            .AddSingleton<IDateTimeService, StandardDateTimeService>()
            .AddTransient<DirectoryResolver, LocalFileSystemDirectoryResolver>()
            .AddTransient<FileResolver, LocalFileSystemFileResolver>()
            .AddSingleton<IdPool, InMemoryStaticIdPool>()
            .AddSingleton<ServerInfo, HostingEnvironmentServerInfo>()
            .AddSingleton<SystemProvidedAssemblyList>(
                new StandardSystemProvidedAssemblyList(
                    systemProvidedAssemblyList
                )
            )
            .AddTransient<IJsonFileLoader, NewtonsoftJsonFileLoader>()
            .AddTransient<IJsonFileSaver, NewtonsoftJsonFileSaver>()
            .AddSingleton<IRandomNumberGenerator, CryptographyRandomNumberGenerator>()
            .AddSingleton<ServerLifetimeState, StandardServerLifetimeState>()
            .AddSingleton<IServerProperty, InMemoryServerProperty>()
        ;

        public struct CoreStartup { }

        public static IApplicationBuilder UseStartingCore(
            this IApplicationBuilder app
        )
        {
            using (var serviceScope = app.CreateServiceScope())
            {
                serviceScope.ServiceProvider
                    .GetService<ILogger<CoreStartup>>()
                    .LogInformation(
                        "Server starting"
                    );
            }
            return app;
        }

        public static IApplicationBuilder UseCore(
            this IApplicationBuilder app
        )
        {
            using (var serviceScope = app.CreateServiceScope())
            {
                serviceScope.ServiceProvider
                    .GetService<IMediator>()
                    .Publish(
                        new FillServerPropertiesEvent()
                    ).GetAwaiter().GetResult();
            }
            return app;
        }

        public static IApplicationBuilder UseFinishStartingCore(
            this IApplicationBuilder app
        )
        {
            using (var serviceScope = app.CreateServiceScope())
            {
                serviceScope.ServiceProvider
                    .GetService<IMediator>()
                    .Send(
                        new FinishServerStartCommand()
                    ).GetAwaiter().GetResult();
                    
                serviceScope.ServiceProvider
                    .GetService<ILogger<CoreStartup>>()
                    .LogInformation(
                        "Server finished starting"
                    );
            }
            return app;
        }
    }
}