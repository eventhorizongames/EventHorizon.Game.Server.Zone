namespace EventHorizon.Game.Server.Zone
{
    using EventHorizon.Zone.System.Combat.Plugin.Skill.State;
    using MediatR;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.AspNetCore.Builder;
    using EventHorizon.Zone.System.Combat.Plugin.Skill.Load;

    public static class SystemCombatPluginSkillExtensions
    {
        public static IServiceCollection AddSystemCombatPluginSkill(
            this IServiceCollection services
        )
        {
            return services
                .AddSingleton<ISkillRepository, SkillRepository>()
            ;
        }
        public static void UseSystemCombatPluginSkill(
            this IApplicationBuilder app
        )
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetService<IMediator>().Send(
                    new LoadSystemCombatPluginSkill()
                ).GetAwaiter().GetResult();
            }
        }
    }
}