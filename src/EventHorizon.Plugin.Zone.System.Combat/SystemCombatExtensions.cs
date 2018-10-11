using EventHorizon.Plugin.Zone.System.Combat.Events;
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
            // services.AddMediatR(typeof(SystemCombatExtensions).Assembly);
        }
        public static void UseSystemCombat(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetService<IMediator>().Publish(new SetupCombatSystemGuiEvent()).GetAwaiter().GetResult();
            }
        }
    }
}