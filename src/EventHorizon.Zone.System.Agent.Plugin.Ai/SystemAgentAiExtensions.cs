using EventHorizon.Zone.System.Agent.Plugin.Ai.Script;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace EventHorizon.Game.Server.Zone
{
    public static class SystemAgentAiExtensions
    {
        public static IServiceCollection AddSystemAgentAi(
            this IServiceCollection services
        )
        {
            return services
                .AddTransient<IScriptServices, ScriptServices>()
            ;
        }
        public static void UseSystemAgentAi(
            this IApplicationBuilder app
        )
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
            }
        }
    }
}