using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace EventHorizon.Game.Server.Zone.Core
{
    public static class SystemCombatPluginEditorExtensions
    {
        public static IServiceCollection AddPluginCombatEditor(
            this IServiceCollection services
        )
        {
            return services;
        }
        public static IApplicationBuilder UsePluginCombatEditor(
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