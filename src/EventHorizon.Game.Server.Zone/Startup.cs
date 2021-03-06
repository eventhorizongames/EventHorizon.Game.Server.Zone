namespace EventHorizon.Game.Server.Zone;

using System.Linq;
using System.Net;
using System.Reflection;

using EventHorizon.BackgroundTasks;
using EventHorizon.Game.I18n;
using EventHorizon.Game.Server.Zone.Controllers;
using EventHorizon.Game.Server.Zone.Core;
using EventHorizon.Game.Server.Zone.Core.JsonConverter;
using EventHorizon.Game.Server.Zone.HealthChecks;
using EventHorizon.Game.Server.Zone.Player;
using EventHorizon.Game.Server.Zone.Setup;
using EventHorizon.Identity;
using EventHorizon.Identity.Policies;
using EventHorizon.Monitoring;
using EventHorizon.Performance;
using EventHorizon.Platform;
using EventHorizon.Server.Core;
using EventHorizon.TimerService;
using EventHorizon.Zone.Core.Entity.Plugin.Reload;
using EventHorizon.Zone.Core.Model.ServerProperty;
using EventHorizon.Zone.System.Admin.ExternalHub;
using EventHorizon.Zone.System.Combat.Plugin.Skill.Editor;
using EventHorizon.Zone.System.Editor.ExternalHub;
using EventHorizon.Zone.System.ModelState;
using EventHorizon.Zone.System.Player.ExternalHub;

using MediatR;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Newtonsoft.Json;

using Prometheus;

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
            ServicePointManager.SecurityProtocol =
                SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
        }
        services.AddHttpClient();

        services
            .AddAuthentication("Bearer")
            .AddIdentityServerAuthentication(
                options =>
                {
                    options.RequireHttpsMetadata =
                        HostingEnvironment.IsProduction() || HostingEnvironment.IsStaging();
                    options.Authority = Configuration["Auth:Authority"];
                    options.ApiName = Configuration["Auth:ApiName"];
                    options.TokenRetriever = WebSocketTokenRetriever.FromHeaderAndQueryString;
                }
            );
        services.AddAuthorization(
            options =>
                options.AddUserIdOrClientIdOrAdminPolicy(
                    Configuration["OwnerDetails:UserId"],
                    Configuration["OwnerDetails:PlatformId"]
                )
        );
        services.AddRazorPages();
        services
            .AddSignalR(
                options =>
                {
                    options.EnableDetailedErrors = HostingEnvironment.IsDevelopment();
                }
            )
            .AddNewtonsoftJsonProtocol(
                config =>
                {
                    config.PayloadSerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    config.PayloadSerializerSettings.Converters.Add(
                        new Newtonsoft.Json.Converters.StringEnumConverter()
                    );
                }
            );

        services.AddResponseCompression(
            options =>
            {
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                    new[] { "application/octet-stream" }
                );
            }
        );

        JsonConvert.DefaultSettings = () =>
            new JsonSerializerSettings { Converters = { new DefaultStringEnumConverter(0) }, };

        services.AddCors(
            options =>
                options.AddPolicy(
                    "CorsPolicy",
                    builder =>
                    {
                        builder
                            .AllowAnyMethod()
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
                    }
                )
        );

        // Health Checks
        services
            .AddHealthChecks()
            .AddCheck<IsServerStartedHealthCheck>(nameof(IsServerStartedHealthCheck));

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
        var systemProvidedAssemblyList = new Assembly[]
        {
            // Base
            typeof(I18nExtensions).Assembly,
            typeof(BackgroundTasksStartupExtensions).Assembly,
            typeof(EventHorizonIdentityExtensions).Assembly,
            typeof(EventHorizonMonitoringExtensions).Assembly,
            // Core
            typeof(CoreExtensions).Assembly,
            typeof(CoreMapExtensions).Assembly,
            typeof(CoreMapPluginEditorExtensions).Assembly,
            typeof(CoreEntityExtensions).Assembly,
            typeof(CoreEntityPluginEditorExtensions).Assembly,
            typeof(CoreEntityPluginReloadExtensions).Assembly,
            typeof(CoreClientExtensions).Assembly,
            typeof(CoreServerActionExtensions).Assembly,
            typeof(CoreReporterExtensions).Assembly,
            // Server
            typeof(Startup).Assembly,
            typeof(ServerCoreExtensions).Assembly,
            // System/Plugin
            typeof(SystemWatcherExtensions).Assembly,
            // - Editor
            typeof(SystemEditorExtensions).Assembly,
            // - Backup
            typeof(SystemBackupExtensions).Assembly,
            // - Data Storage
            typeof(SystemDataStorageExtensions).Assembly,
            // - Artifact Management
            typeof(SystemArtifactManagementExtensions).Assembly,
            // - Admin
            typeof(SystemAdminExtensions).Assembly,
            typeof(SystemAdminExternalHubExtensions).Assembly,
            typeof(SystemAdminPluginCommandExtensions).Assembly,
            // - Asset Server
            typeof(SystemAssetServerExtensions).Assembly,
            // - Server Scripts
            typeof(SystemServerScriptsExtensions).Assembly,
            typeof(SystemServerScriptsPluginSharedExtensions).Assembly,
            typeof(SystemServerScriptsPluginBackgroundTaskExtensions).Assembly,
            typeof(SystemServerScriptsPluginEditorExtensions).Assembly,
            // - GUI
            typeof(SystemGuiExtensions).Assembly,
            typeof(SystemGuiPluginEditorExtensions).Assembly,
            // - Model State
            typeof(SystemModelExtensions).Assembly,
            // - Combat
            typeof(SystemCombatExtensions).Assembly,
            typeof(SystemCombatPluginEditorExtensions).Assembly,
            typeof(SystemCombatPluginSkillExtensions).Assembly,
            typeof(SystemCombatPluginSkillEditorExtensions).Assembly,
            // - Particle
            typeof(SystemParticleExtensions).Assembly,
            typeof(SystemParticlePluginEditorExtensions).Assembly,
            // - Server Module
            typeof(SystemServerModuleExtensions).Assembly,
            typeof(SystemServerModulePluginEditorExtensions).Assembly,
            // - Entity Module
            typeof(SystemEntityModuleExtensions).Assembly,
            typeof(SystemEntityModulePluginEditorExtensions).Assembly,
            // - Agent
            typeof(SystemAgentExtensions).Assembly,
            typeof(SystemAgentPluginEditorExtensions).Assembly,
            typeof(SystemAgentPluginAiExtensions).Assembly,
            typeof(SystemAgentPluginMoveExtensions).Assembly,
            typeof(SystemAgentPluginWildExtensions).Assembly,
            typeof(SystemAgentPluginBehaviorExtensions).Assembly,
            typeof(SystemAgentPluginBehaviorEditorExtensions).Assembly,
            typeof(SystemAgentPluginCompanionExtensions).Assembly,
            // - Client Assets
            typeof(SystemClientAssetsExtensions).Assembly,
            typeof(SystemClientAssetsPluginEditorExtensions).Assembly,
            // - Client Entities
            typeof(SystemClientEntitiesExtensions).Assembly,
            typeof(SystemClientEntitiesPluginEditorExtensions).Assembly,
            // - Client Scripts
            typeof(SystemClientScriptsExtensions).Assembly,
            typeof(SystemClientScriptsPluginSharedExtensions).Assembly,
            typeof(SystemClientScriptsPluginEditorExtensions).Assembly,
            // - Player
            typeof(SystemPlayerExtensions).Assembly,
            typeof(SystemPlayerPluginActionExtensions).Assembly,
            typeof(SystemPlayerPluginEditorExtensions).Assembly,
            // - Interaction
            typeof(SystemInteractionExtensions).Assembly,
            // - Selection
            typeof(SystemSelectionExtensions).Assembly,
            // - Wizard
            typeof(SystemWizardExtensions).Assembly,
            typeof(SystemWizardPluginEditorExtensions).Assembly,
        };

        // Base
        services
            .AddPerformance()
            .AddTimer()
            .AddI18n()
            .AddBackgroundTasksServices()
            .AddEventHorizonIdentity(options => Configuration.GetSection("Auth").Bind(options))
            .AddEventHorizonMonitoring(
                options =>
                {
                    options.Host = Configuration["HOST"] ?? "unset";
                    options.ServerName = Configuration["ServerName"] ?? "Zone";
                }
            );
        // Enabled ApplicationInsights
        // TODO: Move this logic into AddEventHorizonMonitoringApplicationInsights
        if (Configuration.GetValue<bool>("Monitoring:ApplicationInsights:Enabled"))
        {
            services.AddEventHorizonMonitoringApplicationInsights(
                options => Configuration.GetSection("Monitoring:ApplicationInsights").Bind(options)
            );
            systemProvidedAssemblyList = systemProvidedAssemblyList
                .Append(typeof(EventHorizonMonitoringApplicatinInsightsExtensions).Assembly)
                .ToArray();
        }

        // Core
        services
            .AddCore(systemProvidedAssemblyList)
            .AddCoreClient()
            .AddCoreEntity()
            .AddCoreEntityPluginEditor()
            .AddCoreEntityPluginReload()
            .AddCoreMap()
            .AddCoreMapPluginEditor()
            .AddCoreReporter()
            .AddCoreServerAction();

        // Server
        services.AddServerLoad();
        services.AddServerSetup();
        services.AddServerCore(options => Configuration.GetSection("Core").Bind(options));
        services.AddServerPlayer(Configuration);
        services.AddServerAdmin();

        // System/Plugin
        services
            .AddSystemWatcher()
            .AddSystemEditor()
            .AddSystemBackup()
            .AddSystemDataStorage()
            .AddSystemArtifactManagement(
                options => Configuration.GetSection("Systems:ArtifactManagement").Bind(options)
            )
            .AddSystemAdmin()
            .AddSystemAdminPluginCommand()
            .AddSystemAssetServer(options => Configuration.GetSection("Asset").Bind(options))
            .AddSystemServerScripts(
                options => Configuration.GetSection("Systems:ServerScripts").Bind(options)
            )
            .AddSystemServerScriptsPluginShared()
            .AddSystemServerScriptsPluginBackgroundTask()
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
            .AddSystemClientScripts(
                options => Configuration.GetSection("Systems:ClientScripts").Bind(options)
            )
            .AddSystemClientScriptsPluginShared()
            .AddSystemClientScriptsPluginEditor()
            .AddSystemPlayer()
            .AddSystemPlayerPluginAction()
            .AddSystemPlayerPluginEditor()
            .AddSystemInteraction()
            .AddSystemSelection()
            .AddSystemWizard()
            .AddSystemWizardPluginEditor();

        // TODO: MORE CORE SERVER SETUP
        // Has to be last in configuration.
        //  so it picks up the systemProvidedAssemblyList after all services have added to it.
        services.AddMediatR(systemProvidedAssemblyList);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseStartingCore();
        app.UseResponseCompression();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();

        app.UseCors("CorsPolicy");
        app.UseAuthentication();
        app.UseAuthorization();

        UseZonePlatform(app);

        app.UseStaticFiles();
        app.UseHealthChecks("/healthz");
        app.UseEndpoints(
            routes =>
            {
                routes.MapPlatformDetails(
                    options =>
                        options.SetVersion(Configuration[ServerPropertyKeys.APPLICATION_VERSION])
                );
                routes.MapMetrics("/metrics");

                routes.MapHub<AdminHub>("/admin");
                routes.MapHub<PlayerHub>("/playerHub");

                routes.MapHub<SystemEditorHub>("/systemEditor");
                routes.MapHub<SkillsEditorHub>("/skillsEditor");
            }
        );
    }

    // Organized into Base, Core, Server, System, Plugin, Dynamic Plugins
    // Also Sorted based on Load order
    // Base -- These are common functionality; I18n, Identity Integrations, etc...
    // Core -- From the Zone Core Services
    // Server -- From the Zone Server Base Project
    // System -- These are which Systems should be setup for this Zone Server
    // Plugin -- These are Extended functionality for Systems.
    // Dynamically Loaded Plugins -- These are Extra features, but not needed by Systems to function.
    private static IApplicationBuilder UseZonePlatform(IApplicationBuilder app)
    {
        // Base
        app.UseI18n();
        app.UseEventHorizonIdentity();

        // Load Server - Settings
        app.UseServerLoad();

        // Core
        app.UseCore()
            .UseCoreEntity()
            .UseCoreEntityPluginEditor()
            .UseCoreEntityPluginReload()
            .UseCoreMap()
            .UseCoreReporter();

        // Server
        app.UseServerSetup();
        app.UseServerCore();
        app.UseServerPlayer();
        app.UseServerAdmin();

        // System/Plugin
        app.UseSystemWatcher();
        app.UseSystemBackup();
        app.UseSystemEditor();
        app.UseSystemDataStorage();

        app.UseSystemAdmin();
        app.UseSystemAdminPluginCommand();

        app.UseSystemAssetServer();

        app.UseSystemServerScripts();
        app.UseSystemServerScriptsPluginShared();
        app.UseSystemServerScriptsPluginBackgroundTask();
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
        app.UseSystemClientScriptsPluginShared();
        app.UseSystemClientScriptsPluginEditor();

        app.UseSystemPlayer();
        app.UseSystemPlayerPluginAction();
        app.UseSystemPlayerPluginEditor();

        app.UseSystemInteraction();

        app.UseSystemSelection();

        app.UseSystemWizard().UseSystemWizardPluginEditor();

        // Core - Register with Core Server
        app.UseRegisterWithCoreServer();

        app.UseFinishStartingCore();

        return app;
    }
}
