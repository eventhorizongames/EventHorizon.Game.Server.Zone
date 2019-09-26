using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace EventHorizon.Game.Server.Zone
{
    public static class SystemAgentExtensions
    {
        public static IServiceCollection AddSystemAgent(
            this IServiceCollection services
        )
        {
            return services;
        }
        public static void UseSystemAgent(
            this IApplicationBuilder app
        )
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
            }
        }
    }
}