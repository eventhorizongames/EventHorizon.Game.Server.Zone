using System.Linq;
using System.Net;
using System.Reflection;
using EventHorizon.Game.I18n;
using EventHorizon.Game.Server.Zone.Controllers;
using EventHorizon.Game.Server.Zone.Core;
using EventHorizon.Game.Server.Zone.Core.JsonConverter;
using EventHorizon.Game.Server.Zone.Player;
using EventHorizon.Game.Server.Zone.Plugin;
using EventHorizon.Game.Server.Zone.Setup;
using EventHorizon.Identity;
using EventHorizon.Monitoring;
using EventHorizon.Performance;
using EventHorizon.Server.Core;
using EventHorizon.TimerService;
using EventHorizon.Zone.System.Admin.ExternalHub;
using EventHorizon.Zone.System.Combat.Plugin.Skill.Editor;
using EventHorizon.Zone.System.Editor.ExternalHub;
using EventHorizon.Zone.System.ModelState;
using EventHorizon.Zone.System.Player.ExternalHub;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Prometheus;

namespace EventHorizon.Game.Server.Zone
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            HostingEnvironment = env;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment HostingEnvironment { get; }

        // Organized into Base, Core, Server, System, Plugin, Dynamic Plugins
        // Base -- These are common functionality; I18n, Identity Integrations, etc...
        // Core -- From the Zone Core Services
        // Server -- From the Zone Server Base Project
        // System -- These are which Systems should be setup for this Zone Server
        // Plugin -- These are Extended functionality for Systems.
        // Dynamically Loaded Plugins -- These are Extra features, but not needed by Systems to function.
        // THEN - By the package/library order
        // - systemProvidedAssemblyList Registration
        // - Add* Registration
        //
        // I want this file to be able to be generated dynamically from a script, 
        //  that way it can be customized from an external tool and have a very basic setup.
        // TODO: Re-factor ConfigureServices into Chunks
        //  Consolidating the Add* logic from here into the module.
        // TODO: Update extension methods to accept systemProvidedAssemblyList as a parameter.
        // TODO: Update Extension methods to access Configuration as a Parameter.
        public void ConfigureServices(IServiceCollection services)
        {
            // TODO: MAIN SERVER CONFIGURATION
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
            services.AddRazorPages();
            services.AddSignalR()
                .AddNewtonsoftJsonProtocol(config =>
                {
                    config.PayloadSerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    config.PayloadSerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                })
            ;

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
                                    ).GetChildren()
                                    .AsEnumerable()
                                    .Select(
                                        a => a.Value
                                    ).ToArray()
                            ).AllowCredentials();
                    }
                )
            );
            // TODO: END MAIN SERVER CONFIGURATION

            // Organized into Base, Core, Server, System, Plugin, Dynamic Plugins
            // Also Sorted based on Load order
            // Base -- These are common functionality; I18n, Identity Integrations, etc...
            // Core -- From the Zone Core Services
            // Server -- From the Zone Server Base Project
            // System -- These are which Systems should be setup for this Zone Server
            // Plugin -- These are Extended functionality for Systems.
            // Dynamically Loaded Plugins -- These are Extra features, but not needed by Systems to function.

            // This is used system wide services;
            // 1. For Scripting reference resolution.
            // 2. MediatR Handler Setup.
            var systemProvidedAssemblyList = new Assembly[] {
                
                // Base
                typeof(I18nExtensions).Assembly,
                typeof(EventHorizonIdentityExtensions).Assembly,
                typeof(EventHorizonMonitoringExtensions).Assembly,

                // Core
                typeof(CoreExtensions).Assembly,
                typeof(CoreMapExtensions).Assembly,
                typeof(CoreMapPluginEditorExtensions).Assembly,
                typeof(CoreEntityExtensions).Assembly,
                typeof(CoreClientExtensions).Assembly,
                typeof(CoreServerActionExtensions).Assembly,
                typeof(CoreReporterExtensions).Assembly,
                
                // Server
                typeof(Startup).Assembly,
                typeof(ServerCoreExtensions).Assembly,

                // System/Plugin
                typeof(SystemWatcherExtensions).Assembly,
                typeof(SystemEditorExtensions).Assembly,
                typeof(SystemBackupExtensions).Assembly,

                typeof(SystemAdminExtensions).Assembly,
                typeof(SystemAdminExternalHubExtensions).Assembly,
                typeof(SystemAdminPluginCommandExtensions).Assembly,

                typeof(SystemServerScriptsExtensions).Assembly,
                typeof(SystemServerScriptsPluginEditorExtensions).Assembly,

                typeof(SystemGuiExtensions).Assembly,
                typeof(SystemGuiPluginEditorExtensions).Assembly,

                typeof(SystemModelExtensions).Assembly,

                typeof(SystemCombatExtensions).Assembly,
                typeof(SystemCombatPluginEditorExtensions).Assembly,
                typeof(SystemCombatPluginSkillExtensions).Assembly,
                typeof(SystemCombatPluginSkillEditorExtensions).Assembly,

                typeof(SystemParticleExtensions).Assembly,
                typeof(SystemParticlePluginEditorExtensions).Assembly,

                typeof(SystemServerModuleExtensions).Assembly,
                typeof(SystemServerModulePluginEditorExtensions).Assembly,

                typeof(SystemEntityModuleExtensions).Assembly,
                typeof(SystemEntityModulePluginEditorExtensions).Assembly,

                typeof(SystemAgentExtensions).Assembly,
                typeof(SystemAgentPluginEditorExtensions).Assembly,
                typeof(SystemAgentPluginAiExtensions).Assembly,
                typeof(SystemAgentPluginMoveExtensions).Assembly,
                typeof(SystemAgentPluginWildExtensions).Assembly,
                typeof(SystemAgentPluginBehaviorExtensions).Assembly,
                typeof(SystemAgentPluginBehaviorEditorExtensions).Assembly,
                typeof(SystemAgentPluginCompanionExtensions).Assembly,

                typeof(SystemClientAssetsExtensions).Assembly,
                typeof(SystemClientAssetsPluginEditorExtensions).Assembly,

                typeof(SystemClientEntitiesExtensions).Assembly,
                typeof(SystemClientEntitiesPluginEditorExtensions).Assembly,

                typeof(SystemClientScriptsExtensions).Assembly,

                typeof(SystemPlayerExtensions).Assembly,
                typeof(SystemPlayerPluginActionExtensions).Assembly,

                typeof(SystemInteractionExtensions).Assembly,
                
                // Dynamically Loaded Plugins
                typeof(PluginExtensions).Assembly,

                // Game Specific Loading
                typeof(GameExtensions).Assembly,
            };

            // Base
            services
                .AddPerformance()
                .AddTimer()
                .AddI18n()
                .AddEventHorizonIdentity(
                    options => Configuration.GetSection(
                        "Auth"
                    ).Bind(
                        options
                    )
                ).AddEventHorizonMonitoring(
                    options =>
                    {
                        options.Host = Configuration["HOST"] ?? "unset";
                        options.ServerName = Configuration["ServerName"] ?? "Zone";
                    }
                )
            ;
            // Enabled ApplicationInsights
            // TODO: Move this logic into AddEventHorizonMonitoringApplicationInsights
            if (Configuration.GetValue<bool>(
                "Monitoring:ApplicationInsights:Enabled"
            ))
            {
                services.AddEventHorizonMonitoringApplicationInsights(
                    options => Configuration.GetSection(
                        "Monitoring:ApplicationInsights"
                    ).Bind(
                        options
                    )
                );
                systemProvidedAssemblyList.Append(
                    typeof(EventHorizonMonitoringApplicatinInsightsExtensions).Assembly
                );
            }

            // Core
            services
                .AddCore(
                    systemProvidedAssemblyList
                ).AddCoreClient()
                .AddCoreEntity()
                .AddCoreMap()
                .AddCoreMapPluginEditor()
                .AddCoreReporter()
                .AddCoreServerAction();

            // Server
            services.AddServerLoad();
            services.AddServerSetup();
            services.AddServerCore(
                options => Configuration.GetSection(
                    "Core"
                ).Bind(
                    options
                )
            );
            services.AddServerPlayer(Configuration);
            services.AddServerAdmin();

            // System/Plugin
            services
                .AddSystemWatcher()
                .AddSystemEditor()
                .AddSystemBackup()

                .AddSystemAdmin()
                .AddSystemAdminPluginCommand()

                .AddSystemServerScripts()
                .AddSystemServerScriptsPluginEditor()

                .AddSystemGui()
                .AddSystemGuiPluginEditor()

                .AddSystemModelState()

                .AddSystemCombat()
                .AddSystemCombatPluginEditor()
                .AddSystemCombatPluginSkill()
                .AddSystemCombatPluginSkillEditor()

                .AddSystemParticle()
                .AddSystemParticlePluginEditor()

                .AddSystemServerModule()
                .AddSystemServerModulePluginEditor()

                .AddSystemEntityModule()
                .AddSystemEntityModulePluginEditor()

                .AddSystemAgent(Configuration)
                .AddSystemAgentPluginEditor()
                .AddSystemAgentPluginAi()
                .AddSystemAgentPluginMove()
                .AddSystemAgentPluginWild()
                .AddSystemAgentPluginBehavior()
                .AddSystemAgentPluginBehaviorEditor()
                .AddSystemAgentPluginCompanion()

                .AddSystemClientAssets()
                .AddSystemClientAssetsPluginEditor()

                .AddSystemClientEntities()
                .AddSystemClientEntitiesPluginEditor()

                .AddSystemClientScripts()

                .AddSystemPlayer()
                .AddSystemPlayerPluginAction()

                .AddSystemInteraction()
            ;

            // Dynamically Loaded Plugins
            services.AddPlugins(
                HostingEnvironment
            );

            // Game Specific Loading
            services.AddGame();

            // TODO: MORE CORE SERVER SETUP
            // Has to be last in configuration.
            //  so it picks up the systemProvidedAssemblyList after all services have added to it.
            services.AddMediatR(
                systemProvidedAssemblyList
            );
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseStartingCore();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseCors("CorsPolicy");
            app.UseAuthentication();
            app.UseAuthorization();

            // Organized into Base, Core, Server, System, Plugin, Dynamic Plugins
            // Also Sorted based on Load order
            // Base -- These are common functionality; I18n, Identity Integrations, etc...
            // Core -- From the Zone Core Services
            // Server -- From the Zone Server Base Project
            // System -- These are which Systems should be setup for this Zone Server
            // Plugin -- These are Extended functionality for Systems.
            // Dynamically Loaded Plugins -- These are Extra features, but not needed by Systems to function.

            // Base
            app.UseI18n();
            app.UseEventHorizonIdentity();

            // Load Server - Settings 
            app.UseServerLoad();

            // Core
            app.UseCore();
            app.UseCoreMap();
            app.UseCoreReporter();

            // Server 
            app.UseServerSetup();
            app.UseServerCore();
            app.UseServerPlayer();
            app.UseServerAdmin();

            // System/Plugin
            app.UseSystemWatcher();
            app.UseSystemBackup();
            app.UseSystemEditor();

            app.UseSystemAdmin();
            app.UseSystemAdminPluginCommand();

            app.UseSystemServerScripts();
            app.UseSystemServerScriptsPluginEditor();

            app.UseSystemGui();
            app.UseSystemGuiPluginEditor();

            app.UseSystemModelState();

            app.UseSystemCombat();
            app.UseSystemCombatPluginEditor();
            app.UseSystemCombatPluginSkill();
            app.UseSystemCombatPluginSkillEditor();

            app.UseSystemParticle();
            app.UseSystemParticlePluginEditor();

            app.UseSystemServerModule();
            app.UseSystemServerModulePluginEditor();

            app.UseSystemEntityModule();
            app.UseSystemEntityModulePluginEditor();

            app.UseSystemAgent();
            app.UseSystemAgentPluginEditor();
            app.UseSystemAgentPluginAi();
            app.UseSystemAgentPluginMove();
            app.UseSystemAgentPluginWild();
            app.UseSystemAgentPluginBehavior();
            app.UseSystemAgentPluginBehaviorEditor();
            app.UseSystemAgentPluginCompanion();

            app.UseSystemClientAssets();
            app.UseSystemClientAssetsPluginEditor();

            app.UseSystemClientEntities();
            app.UseSystemClientEntitiesPluginEditor();

            app.UseSystemClientScripts();

            app.UseSystemPlayer();
            app.UseSystemPlayerPluginAction();

            app.UseSystemInteraction();

            // Dynamically Loaded Plugins
            app.UsePlugins();

            // Game Specific Loading
            app.UseGame();

            // Core - Register with Core Server
            app.UseRegisterWithCoreServer();

            app.UseFinishStartingCore();

            app.UseStaticFiles();
            app.UseEndpoints(
                routes =>
                {
                    routes.MapMetrics();
                    routes.MapHub<AdminHub>("/admin");
                    routes.MapHub<PlayerHub>("/playerHub");

                    routes.MapHub<SystemEditorHub>("/systemEditor");
                    routes.MapHub<SkillsEditorHub>("/skillsEditor");

                    // routes.MapHub<EditorHub>("/editor");
                }
            );
        }
    }
}