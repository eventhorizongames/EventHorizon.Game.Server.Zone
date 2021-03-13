namespace EventHorizon.Game.Server.Zone.Server.Scripts.SubProcess
{
    using System;
    using System.Threading.Tasks;
    using EventHorizon.Game.Server.Zone.SDK;
    using EventHorizon.Zone.Core.Model.ServerProperty;
    using MediatR;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Serilog;
    using Serilog.Sinks.Elasticsearch;

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
                        .Enrich.WithProperty("ApplicationVersion", ctx.Configuration[ServerPropertyKeys.APPLICATION_VERSION])
                        .ReadFrom.Configuration(ctx.Configuration)
                        .ConfigureElasticsearchLogging(ctx)
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

    public static class SerilogElasticsearchExtensions
    {
        public static LoggerConfiguration ConfigureElasticsearchLogging(
            this LoggerConfiguration loggerConfig,
            HostBuilderContext context
        )
        {
            if (context.Configuration.GetValue<bool>("Serilog:Elasticsearch:Enabled"))
            {
                var sinkOptions = new ElasticsearchSinkOptions(
                    new Uri(
                        context.Configuration["Elasticsearch:Uri"]
                    )
                )
                {
                    ModifyConnectionSettings = conn =>
                    {
                        conn.BasicAuthentication(
                            context.Configuration["Elasticsearch:Username"],
                            context.Configuration["Elasticsearch:Password"]
                        );
                        return conn;
                    }

                };
                context.Configuration.GetSection(
                    "Serilog:Elasticsearch"
                ).Bind(
                    sinkOptions
                );
                return loggerConfig.WriteTo.Elasticsearch(
                    sinkOptions
                );
            }

            return loggerConfig;
        }
    }
}
