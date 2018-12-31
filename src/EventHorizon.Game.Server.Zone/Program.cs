using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;

namespace EventHorizon.Game.Server.Zone
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseSerilog((ctx, cfg) => cfg
                    .Enrich.WithProperty("EnvironmentName", ctx.HostingEnvironment.EnvironmentName)
                    .Enrich.WithProperty("Host", ctx.Configuration["HOST"])
                    .Enrich.WithProperty("ServiceName", "Zone")
                    .ReadFrom.Configuration(ctx.Configuration))
                .Build();
    }
}
