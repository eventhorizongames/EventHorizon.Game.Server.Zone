using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace EventHorizon.Game.Server.Zone.Core
{
    public static class SystemCombatEditorExtensions
    {
        public static IServiceCollection AddSystemCombatEditor(
            this IServiceCollection services
        )
        {
            return services;
        }
        public static IApplicationBuilder UseSystemCombatEditor(
            this IApplicationBuilder app
        )
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
            }
            return app;
        }
    }
}