using EventHorizon.Plugin.Zone.System.Combat.Events;
using EventHorizon.Plugin.Zone.System.Combat.Life;
using EventHorizon.Plugin.Zone.System.Combat.Model.Level;
using EventHorizon.Plugin.Zone.System.Combat.Model.Life;
using EventHorizon.Plugin.Zone.System.Combat.Particle.Event;
using EventHorizon.Plugin.Zone.System.Combat.Skill.Load;
using EventHorizon.Plugin.Zone.System.Combat.Skill.State;
using EventHorizon.Plugin.Zone.System.Combat.State;
using EventHorizon.Plugin.Zone.System.Combat.Timer;
using EventHorizon.TimerService;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventHorizon.Game.Server.Zone.Core
{
    public static class SystemCombatExtensions
    {
        public static void AddSystemCombat(this IServiceCollection services)
        {
            services
                .AddSingleton<ISkillRepository, SkillRepository>()
                .AddSingleton<ISkillEffectScriptRepository, SkillEffectScriptRepository>()
                .AddSingleton<ISkillActionScriptRepository, SkillActionScriptRepository>()
                .AddSingleton<IEntityQueue<ChangeEntityLife>, EntityQueue<ChangeEntityLife>>()
                .AddSingleton<IEntityQueue<EntityLevelUp>, EntityQueue<EntityLevelUp>>()
                .AddTransient<ITimerTask, UpdateEntityLifeTimer>()
                .AddTransient<ITimerTask, EntityLevelUpTimer>()
                .AddSingleton<ILifeStateChange, LifeStateChange>();
        }
        public static void UseSystemCombat(this IApplicationBuilder app)
        {
            using(var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetService<IMediator>().Publish(new SetupCombatSystemGuiEvent()).GetAwaiter().GetResult();
                serviceScope.ServiceProvider.GetService<IMediator>().Publish(new SetupCombatParticleSystemEvent()).GetAwaiter().GetResult();

                serviceScope.ServiceProvider.GetService<IMediator>().Publish(new LoadCombatSkillsEvent()).GetAwaiter().GetResult();
                serviceScope.ServiceProvider.GetService<IMediator>().Publish(new LoadSkillCombatSystemEvent()).GetAwaiter().GetResult();
            }
        }
    }
}