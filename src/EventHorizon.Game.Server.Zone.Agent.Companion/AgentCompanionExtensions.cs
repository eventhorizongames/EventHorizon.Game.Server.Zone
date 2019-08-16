using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace EventHorizon.Game.Server.Zone
{
    public static class AgentCompanionExtensions
    {
        public static IServiceCollection AddAgentCompanion(
            this IServiceCollection services
        )
        {
            return services;
        }
        public static void UseAgentCompanion(
            this IApplicationBuilder app
        )
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
            }
        }
    }
}