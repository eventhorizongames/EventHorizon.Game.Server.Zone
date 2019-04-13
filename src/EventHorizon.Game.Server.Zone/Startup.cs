using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading.Tasks;
using EventHorizon.Game.I18n;
using EventHorizon.Game.Server.Zone.Admin.Bus;
using EventHorizon.Game.Server.Zone.Agent;
using EventHorizon.Game.Server.Zone.Controllers;
using EventHorizon.Game.Server.Zone.Core;
using EventHorizon.Game.Server.Zone.Core.JsonConverter;
using EventHorizon.Game.Server.Zone.Core.Ping;
using EventHorizon.Game.Server.Zone.Core.ServerProperty;
using EventHorizon.Game.Server.Zone.Core.ServerProperty.Impl;
using EventHorizon.Game.Server.Zone.Editor;
using EventHorizon.Game.Server.Zone.Entity;
using EventHorizon.Game.Server.Zone.Gui;
using EventHorizon.Game.Server.Zone.Particle;
using EventHorizon.Game.Server.Zone.Player;
using EventHorizon.Game.Server.Zone.Player.Bus;
using EventHorizon.Game.Server.Zone.Player.State;
using EventHorizon.Game.Server.Zone.Player.State.Impl;
using EventHorizon.Game.Server.Zone.Plugin;
using EventHorizon.Game.Server.Zone.ServerAction;
using EventHorizon.Game.Server.Zone.Setup;
using EventHorizon.Performance;
using EventHorizon.Performance.Impl;
using EventHorizon.Plugin.Zone.System.Combat.Editor;
using EventHorizon.Schedule;
using EventHorizon.TimerService;
using IdentityModel.AspNetCore.OAuth2Introspection;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

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
            services.AddHttpClient();
            services.AddMediatR();

            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.RequireHttpsMetadata = HostingEnvironment.IsProduction() || HostingEnvironment.IsStaging();
                    options.Authority = Configuration["Auth:Authority"];
                    options.ApiName = Configuration["Auth:ApiName"];
                    options.TokenRetriever = WebSocketTokenRetriever.FromHeaderAndQueryString;
                });
            services.AddMvc(options =>
            {
                // options.Filters.Add(typeof(JsonExceptionFilter));
            });
            services.AddSignalR()
                .AddJsonProtocol(config =>
                {
                    config.PayloadSerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    config.PayloadSerializerSettings.Converters.Add(new DefaultStringEnumConverter(0));
                });

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                Converters = { new DefaultStringEnumConverter(0) },
            };

            services.AddCors(options => options.AddPolicy("CorsPolicy",
            builder =>
            {
                builder.AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithOrigins(
                        Configuration
                            .GetSection("Cors:Hosts")
                            .GetChildren()
                            .AsEnumerable()
                            .Select(a => a.Value)
                            .ToArray()
                    )
                    .AllowCredentials();
            }));

            services.AddSingleton<IPerformanceTracker, PerformanceTracker>();
            services.AddI18n();
            services.AddLoad(Configuration);
            services.AddPlayer(Configuration);
            services.AddZoneCore(Configuration);
            services.AddZoneAdmin();
            services.AddServerSetup(Configuration);
            services.AddEntity();
            services.AddAgent(Configuration);
            services.AddGui();
            services.AddParticle();
            services.AddServerAction();

            services.AddScheduler((sender, args) =>
            {
                Console.WriteLine(args.Exception.Message);
                args.SetObserved();
            });
            services.AddTimer();

            // TODO: Remove this after done testing, move to System flow
            services.AddSystemCombat();
            services.AddSystemCombatEditor();
            services.AddSystemModel();
            services.AddSystemServerModule();
            services.AddSystemEntityModule();
            services.AddSystemAgentAi();
            services.AddSystemClientScripts();

            services.AddAgentCompanion();

            services.AddPlugins(HostingEnvironment);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseLoad();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("CorsPolicy");
            app.UseAuthentication();

            app.UseI18n();
            app.UseZoneCore();
            app.UseZoneAdmin();
            app.UseSetupServer();

            // TODO: Remove this after done testing, move to Systems flow
            app.UseSystemModel();
            app.UseSystemCombat();
            app.UseSystemCombatEditor();
            app.UseSystemServerModule();
            app.UseSystemEntityModule();
            app.UseSystemAgentAi();
            app.UseAgentCompanion();
            app.UseSystemClientScripts();

            app.UseAgent();
            app.UseGui();
            app.UseParticle();

            app.UsePlugins();

            app.UseStaticFiles();
            app.UseSignalR(routes =>
            {
                routes.MapHub<PlayerHub>("/playerHub");
                routes.MapHub<EditorHub>("/editor");
                routes.MapHub<SkillsEditorHub>("/skillsEditor");
                routes.MapHub<AgentHub>("/agent");
                routes.MapHub<AdminBus>("/admin");
            });
            app.UseMvc();
        }
    }
}