namespace EventHorizon.Game.Server.Zone
{
    using EventHorizon.Zone.System.Combat.Plugin.Skill.Load;
    using EventHorizon.Zone.System.Combat.Plugin.Skill.State;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

    public static class SystemCombatPluginSkillExtensions
    {
        public static IServiceCollection AddSystemCombatPluginSkill(
            this IServiceCollection services
        ) => services
            .AddSingleton<SkillRepository, InMemorySkillRepository>()
        ;

        public static IApplicationBuilder UseSystemCombatPluginSkill(
            this IApplicationBuilder app
        ) => app.SendMediatorCommand(
            new LoadSystemCombatPluginSkill()
        );
    }
}
