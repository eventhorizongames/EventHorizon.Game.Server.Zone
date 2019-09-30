using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace EventHorizon.Game.Server.Zone.Core
{
    public static class SystemCombatPluginEditorExtensions
    {
        public static IServiceCollection AddSystemCombatPluginEditor(
            this IServiceCollection services
        )
        {
            return services;
        }
        public static IApplicationBuilder UseSystemCombatPluginEditor(
            this IApplicationBuilder app
        )
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                return app;
            }
        }
    }
}