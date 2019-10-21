using EventHorizon.Zone.System.Combat.Life;
using EventHorizon.Zone.System.Combat.Model.Level;
using EventHorizon.Zone.System.Combat.Model.Life;
using EventHorizon.Zone.System.Combat.Script;
using EventHorizon.Zone.System.Combat.Skill.State;
using EventHorizon.Zone.System.Combat.State;
using EventHorizon.Zone.System.Combat.Timer;
using EventHorizon.TimerService;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using EventHorizon.Zone.System.Combat.Load;
using Microsoft.AspNetCore.Builder;

namespace EventHorizon.Game.Server.Zone.Core
{
    public static class SystemCombatExtensions
    {
        public static IServiceCollection AddSystemCombat(
            this IServiceCollection services
        )
        {
            return services
                .AddSingleton<ISkillRepository, SkillRepository>()
                .AddSingleton<ISkillValidatorScriptRepository, SkillValidatorScriptRepository>()
                .AddSingleton<IEntityQueue<ChangeEntityLife>, EntityQueue<ChangeEntityLife>>()
                .AddSingleton<IEntityQueue<EntityLevelUp>, EntityQueue<EntityLevelUp>>()
                .AddSingleton<ILifeStateChange, LifeStateChange>()

                .AddTransient<IScriptServices, ScriptServices>()
                .AddTransient<ITimerTask, UpdateEntityLifeTimer>()
                .AddTransient<ITimerTask, EntityLevelUpTimer>()
            ;
        }
        public static void UseSystemCombat(
            this IApplicationBuilder app
        )
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetService<IMediator>().Publish(
                    new LoadCombatSystemEvent()
                ).GetAwaiter().GetResult();
            }
        }
    }
}