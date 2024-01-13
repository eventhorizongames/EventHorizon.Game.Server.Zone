namespace EventHorizon.Game.Server.Zone.Client.Scripts.SubProcess
{
    using System;
    using System.Reflection;
    using System.Threading.Tasks;
    using EventHorizon.Zone.Core.Model.ServerProperty;
    using MediatR;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Serilog;
    using Serilog.Sinks.Elasticsearch;

    class Program
    {
        public static string Identifier = "client_script_subprocess";

        static Task Main(string[] args) => CreateHostBuilder(args).Build().RunAsync();

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog(
                    (ctx, cfg) =>
                        cfg.Enrich.WithProperty(
                            "EnvironmentName",
                            ctx.HostingEnvironment.EnvironmentName
                        )
                            .Enrich.WithProperty("ProcessIdentifier", Identifier)
                            .Enrich.WithProperty(
                                "PlatformId",
                                ctx.Configuration["OwnerDetails:PlatformId"]
                            )
                            .Enrich.WithProperty("Host", ctx.Configuration["HOST"])
                            .Enrich.WithProperty("ServiceName", "Zone")
                            .Enrich.WithProperty(
                                "ApplicationVersion",
                                ctx.Configuration[ServerPropertyKeys.APPLICATION_VERSION]
                            )
                            .ReadFrom.Configuration(ctx.Configuration)
                            .ConfigureElasticsearchLogging(ctx)
                )
                .ConfigureServices(
                    (host, services) =>
                        services
                            .AddHttpClient()
                            .AddMediatR(
                                opts =>
                                    opts.RegisterServicesFromAssemblies(
                                        [
                                            typeof(Program).Assembly,
                                            typeof(CoreExtensions).Assembly,
                                            typeof(SystemClientScriptsPluginSharedExtensions).Assembly,
                                            typeof(SystemClientScriptsPluginCompilerExtensions).Assembly,
                                        ]
                                    )
                            )
                            .AddCore(Array.Empty<Assembly>())
                            .AddSystemClientScriptsPluginCompiler(
                                options =>
                                    host.Configuration.GetSection(
                                        "Plugins:ClientScriptsPluginCompiler"
                                    )
                                        .Bind(options)
                            )
                            .AddSystemClientScriptsPluginShared()
                            .AddHostedService<Worker>()
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
                    new Uri(context.Configuration["Elasticsearch:Uri"])
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
                context.Configuration.GetSection("Serilog:Elasticsearch").Bind(sinkOptions);
                return loggerConfig.WriteTo.Elasticsearch(sinkOptions);
            }

            return loggerConfig;
        }
    }
}
