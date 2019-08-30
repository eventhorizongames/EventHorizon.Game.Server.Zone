using System;
using System.Linq;
using System.Net;
using System.Reflection;
using EventHorizon.Game.I18n;
using EventHorizon.Game.Server.Zone.Admin.Bus;
using EventHorizon.Game.Server.Zone.Agent;
using EventHorizon.Game.Server.Zone.Controllers;
using EventHorizon.Game.Server.Zone.Core;
using EventHorizon.Game.Server.Zone.Core.JsonConverter;
using EventHorizon.Game.Server.Zone.Editor;
using EventHorizon.Game.Server.Zone.Entity;
using EventHorizon.Game.Server.Zone.Particle;
using EventHorizon.Game.Server.Zone.Player;
using EventHorizon.Game.Server.Zone.Player.Bus;
using EventHorizon.Game.Server.Zone.Plugin;
using EventHorizon.Game.Server.Zone.ServerAction;
using EventHorizon.Game.Server.Zone.Setup;
using EventHorizon.Performance;
using EventHorizon.Performance.Impl;
using EventHorizon.Plugin.Zone.System.Combat.Editor;
using EventHorizon.Schedule;
using EventHorizon.TimerService;
using EventHorizon.Zone.System.Editor.ExternalHub;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

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
                System.Net.ServicePointManager.SecurityProtocol =
                    SecurityProtocolType.Tls12
                    | SecurityProtocolType.Tls11
                    | SecurityProtocolType.Tls;
            }
            services.AddHttpClient();

            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.RequireHttpsMetadata =
                        HostingEnvironment.IsProduction()
                        || HostingEnvironment.IsStaging();
                    options.Authority = Configuration["Auth:Authority"];
                    options.ApiName = Configuration["Auth:ApiName"];
                    options.TokenRetriever = WebSocketTokenRetriever.FromHeaderAndQueryString;
                });
            services.AddMvc();
            services.AddSignalR()
                .AddJsonProtocol(config =>
                {
                    config.PayloadSerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    config.PayloadSerializerSettings
                        .Converters.Add(
                            new DefaultStringEnumConverter(0)
                        );
                });

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                Converters = { new DefaultStringEnumConverter(0) },
            };

            services.AddCors(
                options => options.AddPolicy(
                    "CorsPolicy",
                    builder =>
                    {
                        builder.AllowAnyMethod()
                            .AllowAnyHeader()
                            .WithOrigins(
                                Configuration
                                    .GetSection(
                                        "Cors:Hosts"
                                    ).GetChildren(

                                    ).AsEnumerable(

                                    ).Select(
                                        a => a.Value
                                    ).ToArray()
                            )
                            .AllowCredentials();
                    }
                )
            );

            services.AddSingleton<IPerformanceTracker, PerformanceTracker>();

            // Core Services
            services.AddLoad(Configuration);
            services.AddPlayer(Configuration);
            services.AddZoneCore(Configuration);
            services.AddServerSetup(Configuration);
            services.AddEntity();

            services.AddZoneAdmin();
            services.AddAgent(Configuration);
            services.AddParticle();
            services.AddServerAction();

            services.AddScheduler((sender, args) =>
            {
                Console.WriteLine(args.Exception.Message);
                args.SetObserved();
            });
            services.AddTimer();

            // TODO: Remove this after done testing, move to System flow
            services.AddSystemServer();

            // External Services, Systems, and Plugins
            services.AddI18n()
                .AddSystemEditor()
                .AddSystemBackup()
                .AddSystemGui()
                .AddPluginGuiEditor()
                .AddSystemCombat()
                .AddSystemCombatEditor()
                .AddPluginParticleEditor()
                .AddSystemModel()
                .AddSystemServerModule()
                .AddPluginServerModuleEditor()
                .AddSystemEntityModule()
                .AddPluginEntityModuleEditor()
                .AddSystemAgent()
                .AddSystemAgentAi()
                .AddSystemAgentBehavior()
                .AddPluginAgentBehaviorEditor()
                .AddSystemClientAssets()
                .AddPluginClientAssetsEditor()
                .AddSystemClientEntities()
                .AddPluginClientEntitiesEditor()
                .AddSystemClientScripts()
                .AddSystemPlayer()
                .AddAgentCompanion()
                .AddPlugins(HostingEnvironment)
                .AddPluginInteraction();

            // To be moved into extension startup
            var extensionAssemblyList = new Assembly[] {
                    typeof(I18nExtensions).Assembly,
                    typeof(Startup).Assembly,
                    typeof(SystemEditorExtensions).Assembly,
                    typeof(SystemBackupExtensions).Assembly,
                    typeof(SystemCombatExtensions).Assembly,
                    typeof(SystemCombatEditorExtensions).Assembly,
                    typeof(PluginParticleEditorExtensions).Assembly,
                    typeof(SystemModelExtensions).Assembly,
                    typeof(SystemServerModuleExtensions).Assembly,
                    typeof(PluginServerModuleEditorExtensions).Assembly,
                    typeof(SystemEntityModuleExtensions).Assembly,
                    typeof(PluginEntityModuleEditorExtensions).Assembly,
                    typeof(SystemAgentExtensions).Assembly,
                    typeof(SystemAgentAiExtensions).Assembly,
                    typeof(SystemClientAssetsExtensions).Assembly,
                    typeof(PluginClientAssetsEditorExtensions).Assembly,
                    typeof(SystemClientEntitiesExtensions).Assembly,
                    typeof(PluginClientEntitiesEditorExtensions).Assembly,
                    typeof(SystemClientScriptsExtensions).Assembly,
                    typeof(SystemPlayerExtensions).Assembly,
                    typeof(SystemGuiExtensions).Assembly,
                    typeof(PluginGuiEditorExtensions).Assembly,
                    typeof(SystemAgentBehaviorExtensions).Assembly,
                    typeof(PluginAgentBehaviorEditorExtensions).Assembly,
                    typeof(AgentCompanionExtensions).Assembly,
                    typeof(PluginExtensions).Assembly,
                    typeof(PluginInteractionExtensions).Assembly,
            };
            
            "".Reverse().Where(a => char.IsLetter(a)).ToString();
            services
                .AddMediatR(
                    extensionAssemblyList
                );
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
            app.UseSetupServer();

            // TODO: Remove this after done testing, move to Systems flow
            // The systems flow will automatically load these systems from a folder,
            //  just like the plugin flow, which is about the same.
            app.UseSystemServer();
            app.UseSystemEditor();
            app.UseSystemBackup();
            app.UseSystemGui();
            app.UsePluginGuiEditor();
            app.UseSystemModel();
            app.UseSystemCombat();
            app.UseSystemCombatEditor();
            app.UsePluginParticleEditor();
            app.UseSystemServerModule();
            app.UsePluginServerModuleEditor();
            app.UseSystemEntityModule();
            app.UsePluginEntityModuleEditor();
            app.UseSystemAgent();
            app.UseSystemAgentAi();
            app.UseSystemAgentBehavior();
            app.UsePluginAgentBehaviorEditor();
            app.UseSystemClientAssets();
            app.UsePluginClientAssetsEditor();
            app.UseSystemClientEntities();
            app.UsePluginClientEntitiesEditor();
            app.UseSystemClientScripts();
            app.UseSystemPlayer();

            // Need to load UseSystemServer, needs scripts 
            app.UseZoneAdmin();

            app.UseAgentCompanion();

            app.UseAgent();
            app.UseParticle();

            app.UsePlugins();
            app.UsePluginInteraction();

            app.UseStaticFiles();
            app.UseSignalR(routes =>
            {
                routes.MapHub<PlayerHub>("/playerHub");
                routes.MapHub<EditorHub>("/editor");
                routes.MapHub<SkillsEditorHub>("/skillsEditor");
                routes.MapHub<AgentHub>("/agent");
                routes.MapHub<AdminBus>("/admin");

                routes.MapHub<SystemEditorHub>("/systemEditor");
            });
            app.UseMvc();
        }
    }
}