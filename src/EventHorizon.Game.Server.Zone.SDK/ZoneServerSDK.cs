namespace EventHorizon.Game.Server.Zone.SDK
{
    using System.Reflection;
    using EventHorizon.Game.I18n;
    using EventHorizon.Game.Server.Zone.Core;
    using EventHorizon.Identity;
    using EventHorizon.Monitoring;
    using EventHorizon.Server.Core;
    using EventHorizon.Zone.System.Admin.ExternalHub;
    using EventHorizon.Zone.System.ModelState;

    public static class ZoneServerSDK
    {
        public static Assembly[] SystemProvidedAssemblyList => new Assembly[]
        {
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
            typeof(ServerCoreExtensions).Assembly,

            // System/Plugin
            typeof(SystemWatcherExtensions).Assembly,
            typeof(SystemEditorExtensions).Assembly,
            typeof(SystemBackupExtensions).Assembly,
            typeof(SystemDataStorageExtensions).Assembly,

            typeof(SystemAdminExtensions).Assembly,
            typeof(SystemAdminExternalHubExtensions).Assembly,
            typeof(SystemAdminPluginCommandExtensions).Assembly,

            typeof(SystemServerScriptsExtensions).Assembly,
            typeof(SystemServerScriptsPluginSharedExtensions).Assembly,
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
            typeof(SystemClientScriptsPluginEditorExtensions).Assembly,

            typeof(SystemPlayerExtensions).Assembly,
            typeof(SystemPlayerPluginActionExtensions).Assembly,

            typeof(SystemInteractionExtensions).Assembly,
                
            // Game Specific Loading
            typeof(GameExtensions).Assembly,
        };
    }
}
