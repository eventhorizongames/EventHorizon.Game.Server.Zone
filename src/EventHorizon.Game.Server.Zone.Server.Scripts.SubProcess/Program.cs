﻿namespace EventHorizon.Game.Server.Zone.Server.Scripts.SubProcess
{
    using System;
    using System.Threading.Tasks;
    using EventHorizon.Game.Server.Zone.SDK;
    using MediatR;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Serilog;

    class Program
    {
        public static string Identifier = "server_script_subprocess";
        static Task Main(string[] args) =>
            CreateHostBuilder(args).Build().RunAsync();

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog((ctx, cfg) => cfg
                        .Enrich.WithProperty("EnvironmentName", ctx.HostingEnvironment.EnvironmentName)
                        .Enrich.WithProperty("ProcessIdentifier", Identifier)
                        .Enrich.WithProperty("PlatformId", ctx.Configuration["OwnerDetails:PlatformId"])
                        .Enrich.WithProperty("Host", ctx.Configuration["HOST"])
                        .Enrich.WithProperty("ServiceName", "Zone")
                        .ReadFrom.Configuration(ctx.Configuration)
                ).ConfigureServices(
                    (host, services) => services
                        .AddMediatR(
                            new Type[]
                            {
                                typeof(Program),
                                typeof(CoreExtensions),
                                typeof(SystemServerScriptsPluginSharedExtensions),
                                typeof(SystemServerScriptsPluginCompilerExtensions),
                            }
                        ).AddCore(
                            // List of all SDK/API projects for Server Scripts
                            ZoneServerSDK.SystemProvidedAssemblyList
                        )
                        .AddSystemServerScriptsPluginShared()
                        .AddSystemServerScriptsPluginCompiler()
                        .AddHostedService<WorkerService>()
                );
    }
}
