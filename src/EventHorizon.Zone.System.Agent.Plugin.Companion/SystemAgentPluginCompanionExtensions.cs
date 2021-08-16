using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace EventHorizon.Game.Server.Zone
{
    public static class SystemAgentPluginCompanionExtensions
    {
        public static IServiceCollection AddSystemAgentPluginCompanion(
            this IServiceCollection services
        )
        {
            return services;
        }
        public static IApplicationBuilder UseSystemAgentPluginCompanion(
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
