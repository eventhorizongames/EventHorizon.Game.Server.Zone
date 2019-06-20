using EventHorizon.Plugin.Zone.Agent.Ai.Script;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace EventHorizon.Game.Server.Zone.Core
{
    public static class SystemAgentAiExtensions
    {
        public static void AddSystemAgentAi(this IServiceCollection services)
        {
            services
                .AddTransient<IScriptServices, ScriptServices>();
        }
        public static void UseSystemAgentAi(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
            }
        }
    }
}