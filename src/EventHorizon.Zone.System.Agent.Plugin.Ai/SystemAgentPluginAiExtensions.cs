using EventHorizon.Zone.System.Agent.Plugin.Ai.Script;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace EventHorizon.Game.Server.Zone
{
    public static class SystemAgentPluginAiExtensions
    {
        public static IServiceCollection AddSystemAgentPluginAi(
            this IServiceCollection services
        )
        {
            return services
                .AddTransient<IScriptServices, ScriptServices>()
            ;
        }
        public static void UseSystemAgentPluginAi(
            this IApplicationBuilder app
        )
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
            }
        }
    }
}