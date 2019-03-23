using EventHorizon.Plugin.Zone.System.Combat.Events;
using EventHorizon.Plugin.Zone.System.Combat.Life;
using EventHorizon.Plugin.Zone.System.Combat.Model.Level;
using EventHorizon.Plugin.Zone.System.Combat.Model.Life;
using EventHorizon.Plugin.Zone.System.Combat.Particle.Event;
using EventHorizon.Plugin.Zone.System.Combat.Skill.Load;
using EventHorizon.Plugin.Zone.System.Combat.Script;
using EventHorizon.Plugin.Zone.System.Combat.Skill.State;
using EventHorizon.Plugin.Zone.System.Combat.State;
using EventHorizon.Plugin.Zone.System.Combat.Timer;
using EventHorizon.TimerService;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using EventHorizon.Plugin.Zone.System.Combat.Load;

namespace EventHorizon.Game.Server.Zone.Core
{
    public static class SystemCombatEditorExtensions
    {
        public static IServiceCollection AddSystemCombatEditor(this IServiceCollection services)
        {
            return services;
        }
        public static IApplicationBuilder UseSystemCombatEditor(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
            }
            return app;
        }
    }
}