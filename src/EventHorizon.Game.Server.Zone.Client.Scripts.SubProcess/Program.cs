namespace EventHorizon.Game.Server.Zone.Client.Scripts.SubProcess
{
    using System;
    using System.Reflection;
    using System.Threading.Tasks;
    using EventHorizon.Zone.System.Client.Scripts.Plugin.Compiler.Api;
    using EventHorizon.Zone.System.Client.Scripts.Plugin.Compiler.Consolidate;
    using EventHorizon.Zone.System.Client.Scripts.Plugin.Compiler.CSharp;
    using MediatR;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Serilog;

    class Program
    {
        public static string Identifier = "client_script_subprocess";
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
                                typeof(SystemClientScriptsPluginCompilerExtensions),
                            }
                        ).AddCore(
                            Array.Empty<Assembly>()
                        ).AddSystemClientScriptsPluginCompiler(
                            options => host.Configuration.GetSection(
                                "Plugins:ClientScriptsPluginCompiler"
                            ).Bind(
                                options
                            )
                        )
                        .AddSingleton<ClientScriptsConsolidator, StandardClientScriptsConsolidator>()
                        .AddSingleton<ClientScriptCompiler, ClientScriptCompilerForCSharp>()
                        .AddHostedService<Worker>()
                );
    }
}
