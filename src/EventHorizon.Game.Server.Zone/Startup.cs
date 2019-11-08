using System;
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
using EventHorizon.Performance;
using EventHorizon.Performance.Impl;
using EventHorizon.Server.Core;
using EventHorizon.TimerService;
using EventHorizon.Zone.System.Admin.ExternalHub;
using EventHorizon.Zone.System.Combat.Plugin.Editor.Skills;
using EventHorizon.Zone.System.Editor.ExternalHub;
using EventHorizon.Zone.System.ModelState;
using EventHorizon.Zone.System.Player.ExternalHub;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Extensions.Hosting;

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
            services.AddRazorPages();
            services.AddSignalR()
                .AddNewtonsoftJsonProtocol(config =>
                {
                    config.PayloadSerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    // This might cause errors in the SignalR connection request/reposes, since they will not longer be default of 0
                    // config.PayloadSerializerSettings
                    //     .Converters.Add(
                    //         new DefaultStringEnumConverter(0)
                    //     );
                    config.PayloadSerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
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

                // Core
                typeof(CoreExtensions).Assembly,
                typeof(CoreMapExtensions).Assembly,
                typeof(CoreEntityExtensions).Assembly,
                typeof(CoreClientExtensions).Assembly,
                typeof(CoreServerActionExtensions).Assembly,
                typeof(CoreReporterExtensions).Assembly,
                
                // Server
                typeof(Startup).Assembly,

                // System/Plugin
                typeof(SystemWatcherExtensions).Assembly,
                typeof(SystemEditorExtensions).Assembly,
                typeof(SystemBackupExtensions).Assembly,

                typeof(SystemAdminExtensions).Assembly,
                typeof(SystemAdminExternalHubExtensions).Assembly,
                typeof(SystemAdminPluginCommandExtensions).Assembly,

                typeof(SystemServerScriptsExtensions).Assembly,

                typeof(SystemGuiExtensions).Assembly,
                typeof(SystemGuiPluginEditorExtensions).Assembly,

                typeof(SystemModelExtensions).Assembly,

                typeof(SystemCombatExtensions).Assembly,
                typeof(SystemCombatPluginEditorExtensions).Assembly,

                typeof(SystemParticleExtensions).Assembly,
                typeof(SystemParticlePluginEditorExtensions).Assembly,

                typeof(SystemServerModuleExtensions).Assembly,
                typeof(SystemServerModulePluginEditorExtensions).Assembly,

                typeof(SystemEntityModuleExtensions).Assembly,
                typeof(SystemEntityModulePluginEditorExtensions).Assembly,

                typeof(SystemAgentExtensions).Assembly,
                typeof(SystemAgentPluginAiExtensions).Assembly,
                typeof(SystemAgentPluginMoveExtensions).Assembly,
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
            };

            // Base
            services
                .AddSingleton<IPerformanceTracker, PerformanceTracker>()
                .AddTimer()
                .AddI18n()
                .AddEventHorizonIdentity(
                    options => Configuration.GetSection(
                        "Auth"
                    ).Bind(
                        options
                    )
                ).AddServerCoreExternal(
                    options => Configuration.GetSection(
                        "Core"
                    ).Bind(
                        options
                    )
                );

            // Core
            services
                .AddCore(
                    systemProvidedAssemblyList
                )
                .AddCoreClient()
                .AddCoreEntity()
                .AddCoreMap()
                .AddCoreReporter()
                .AddCoreServerAction();

            // Server
            services.AddServerLoad();
            services.AddServerSetup();
            services.AddServerCore();
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

                .AddSystemGui()
                .AddSystemGuiPluginEditor()

                .AddSystemModelState()

                .AddSystemCombat()
                .AddSystemCombatPluginEditor()

                .AddSystemParticle()
                .AddSystemParticlePluginEditor()

                .AddSystemServerModule()
                .AddSystemServerModulePluginEditor()

                .AddSystemEntityModule()
                .AddSystemEntityModulePluginEditor()

                .AddSystemAgent(Configuration)
                .AddSystemAgentPluginAi()
                .AddSystemAgentPluginMove()
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
            services
                .AddPlugins(HostingEnvironment);

            services
                .AddMediatR(
                    systemProvidedAssemblyList
                );
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {

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

            // Core
            app.UseCore();

            // Server
            app.UseServerLoad();
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

            app.UseSystemGui();
            app.UseSystemGuiPluginEditor();

            app.UseSystemModelState();

            app.UseSystemCombat();
            app.UseSystemCombatPluginEditor();

            app.UseSystemParticle();
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

            app.UseFinishStartingCore();

            app.UseStaticFiles();
            app.UseEndpoints(
                routes =>
                {
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