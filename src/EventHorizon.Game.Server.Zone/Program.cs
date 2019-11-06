using Microsoft.AspNetCore.Hosting;
using Serilog;
using Microsoft.Extensions.Hosting;

namespace EventHorizon.Game.Server.Zone
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Build().Run();
        }

        public static IHostBuilder BuildWebHost(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();

                    webBuilder.UseSerilog((ctx, cfg) => cfg
                        .Enrich.WithProperty("EnvironmentName", ctx.HostingEnvironment.EnvironmentName)
                        .Enrich.WithProperty("Host", ctx.Configuration["HOST"])
                        .Enrich.WithProperty("ServiceName", "Zone")
                        .ReadFrom.Configuration(ctx.Configuration));
                });
    }
}
