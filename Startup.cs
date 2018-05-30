using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Core;
using EventHorizon.Game.Server.Zone.Core.Factory;
using EventHorizon.Game.Server.Zone.Core.Factory.Impl;
using EventHorizon.Game.Server.Zone.Core.ServerProperty;
using EventHorizon.Game.Server.Zone.Core.ServerProperty.Impl;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EventHorizon.Game.Server.Zone
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            HostingEnvironment = env;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment HostingEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            if (HostingEnvironment.IsDevelopment())
            {
                // Enabled TLS 1.2
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            }
            services.AddMediatR(typeof(Startup).GetTypeInfo().Assembly);

            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.RequireHttpsMetadata = HostingEnvironment.IsProduction() || HostingEnvironment.IsStaging();
                    options.Authority = Configuration["Auth:Authority"];
                    options.ApiName = Configuration["Auth:ApiName"];
                });
            services.AddMvc(options =>
            {
                // options.Filters.Add(typeof(JsonExceptionFilter));
            });

            services.AddZoneCore(Configuration);
            services.AddSingleton<IHttpClientFactory, HttpClientFactory>();
            services.AddSingleton<IServerProperty, ServerProperty>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseZoneCore();

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}