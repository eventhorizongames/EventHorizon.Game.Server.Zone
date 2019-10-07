using System;
using System.Linq;
using System.Net;
using System.Reflection;
using EventHorizon.Game.I18n;
using EventHorizon.Game.Server.Zone.Admin.Bus;
using EventHorizon.Game.Server.Zone.Controllers;
using EventHorizon.Game.Server.Zone.Core;
using EventHorizon.Game.Server.Zone.Core.JsonConverter;
using EventHorizon.Game.Server.Zone.Entity;
using EventHorizon.Game.Server.Zone.Particle;
using EventHorizon.Game.Server.Zone.Player;
using EventHorizon.Game.Server.Zone.Player.Bus;
using EventHorizon.Game.Server.Zone.Plugin;
using EventHorizon.Game.Server.Zone.Setup;
using EventHorizon.Identity;
using EventHorizon.Performance;
using EventHorizon.Performance.Impl;
using EventHorizon.Schedule;
using EventHorizon.Server.Core;
using EventHorizon.TimerService;
using EventHorizon.Zone.System.Combat.Plugin.Editor.Skills;
using EventHorizon.Zone.System.Editor.ExternalHub;
using EventHorizon.Zone.System.ModelState;
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
            services.AddServerSetup(Configuration);
            services.AddZoneCore(Configuration);
            services.AddCoreMap();
            services.AddCoreEntity();
            services.AddPlayer(Configuration);

            services.AddZoneAdmin();
            services.AddParticle();
            services.AddCoreServerAction();

            services.AddScheduler((sender, args) =>
            {
                Console.WriteLine(args.Exception.Message);
                args.SetObserved();
            });
            services.AddTimer();

            // External Services, Systems, and Plugins
            services
                .AddI18n()
                .AddEventHorizonIdentity(Configuration)
                .AddServerCoreExternal()
                .AddSystemServerScripts()
                .AddSystemEditor()
                .AddSystemBackup()
                .AddSystemGui()
                .AddSystemGuiPluginEditor()
                .AddSystemCombat()
                .AddSystemCombatPluginEditor()
                .AddSystemParticlePluginEditor()
                .AddSystemModelState()
                .AddSystemServerModule()
                .AddSystemServerModulePluginEditor()
                .AddSystemEntityModule()
                .AddSystemEntityModulePluginEditor()
                .AddSystemAgent(Configuration)
                .AddSystemAgentPluginAi()
                .AddSystemAgentPluginMove()
                .AddSystemAgentPluginBehavior()
                .AddSystemAgentPluginBehaviorEditor()
                .AddSystemClientAssets()
                .AddSystemClientAssetsPluginEditor()
                .AddSystemClientEntities()
                .AddSystemClientEntitiesPluginEditor()
                .AddSystemClientScripts()
                .AddSystemPlayer()
                .AddSystemAgentPluginCompanion()
                .AddPlugins(HostingEnvironment)
                .AddSystemInteraction();

            // To be moved into extension startup
            var extensionAssemblyList = new Assembly[] {
                    typeof(Startup).Assembly,
                    typeof(CoreMapExtensions).Assembly,
                    typeof(CoreEntityExtensions).Assembly,
                    typeof(CoreServerActionExtensions).Assembly,
                    typeof(I18nExtensions).Assembly,
                    typeof(EventHorizonIdentityExtensions).Assembly,
                    typeof(PluginExtensions).Assembly,
                    typeof(SystemServerScriptsExtensions).Assembly,
                    typeof(SystemEditorExtensions).Assembly,
                    typeof(SystemBackupExtensions).Assembly,
                    typeof(SystemCombatExtensions).Assembly,
                    typeof(SystemCombatPluginEditorExtensions).Assembly,
                    typeof(SystemParticlePluginEditorExtensions).Assembly,
                    typeof(SystemModelExtensions).Assembly,
                    typeof(SystemServerModuleExtensions).Assembly,
                    typeof(SystemServerModulePluginEditorExtensions).Assembly,
                    typeof(SystemEntityModuleExtensions).Assembly,
                    typeof(SystemEntityModulePluginEditorExtensions).Assembly,
                    typeof(SystemAgentExtensions).Assembly,
                    typeof(SystemAgentPluginAiExtensions).Assembly,
                    typeof(SystemAgentPluginMoveExtensions).Assembly,
                    typeof(SystemAgentPluginCompanionExtensions).Assembly,
                    typeof(SystemAgentPluginBehaviorExtensions).Assembly,
                    typeof(SystemAgentPluginBehaviorEditorExtensions).Assembly,
                    typeof(SystemClientAssetsExtensions).Assembly,
                    typeof(SystemClientAssetsPluginEditorExtensions).Assembly,
                    typeof(SystemClientEntitiesExtensions).Assembly,
                    typeof(SystemClientEntitiesPluginEditorExtensions).Assembly,
                    typeof(SystemClientScriptsExtensions).Assembly,
                    typeof(SystemPlayerExtensions).Assembly,
                    typeof(SystemGuiExtensions).Assembly,
                    typeof(SystemGuiPluginEditorExtensions).Assembly,
                    typeof(SystemInteractionExtensions).Assembly,
            };

            services
                .AddMediatR(
                    extensionAssemblyList
                );
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseLoad();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("CorsPolicy");
            app.UseAuthentication();

            // TODO: Look at organizing this into Platform, Systems, Plugin order
            // Platform -- From the Zone Base Project
            // System -- These are which Systems should be setup for this Zone Server
            //  The systems flow should automatically load these systems.
            // Plugin -- These are Extra features, but not needed by Systems to function.
            app.UseI18n();
            app.UseEventHorizonIdentity();
            app.UseZoneCore();
            app.UseSetupServer();

            app.UseSystemServerScripts();
            app.UseSystemEditor();
            app.UseSystemBackup();

            app.UseSystemGui();
            app.UseSystemGuiPluginEditor();

            app.UseSystemModelState();

            app.UseSystemCombat();
            app.UseSystemCombatPluginEditor();

            app.UseSystemParticlePluginEditor();

            app.UseSystemServerModule();
            app.UseSystemServerModulePluginEditor();

            app.UseSystemEntityModule();
            app.UseSystemEntityModulePluginEditor();

            app.UseSystemAgent();
            app.UseSystemAgentPluginAi();
            app.UseSystemAgentPluginMove();
            app.UseSystemAgentPluginBehavior();
            app.UseSystemAgentPluginBehaviorEditor();

            app.UseSystemClientAssets();
            app.UseSystemClientAssetsPluginEditor();

            app.UseSystemClientEntities();
            app.UseSystemClientEntitiesPluginEditor();

            app.UseSystemClientScripts();

            app.UseSystemPlayer();

            app.UseZoneAdmin();

            app.UseSystemAgentPluginCompanion();

            app.UseParticle();

            app.UsePlugins();
            app.UseSystemInteraction();

            app.UseStaticFiles();
            app.UseSignalR(routes =>
            {
                routes.MapHub<AdminBus>("/admin");
                routes.MapHub<PlayerHub>("/playerHub");

                routes.MapHub<SystemEditorHub>("/systemEditor");
                routes.MapHub<SkillsEditorHub>("/skillsEditor");

                // routes.MapHub<EditorHub>("/editor");
            });
            app.UseMvc();
        }
    }
}