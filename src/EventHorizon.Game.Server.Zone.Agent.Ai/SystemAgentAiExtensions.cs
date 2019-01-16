using EventHorizon.Game.Server.Zone.Agent.Ai.LoadRoutine;
using EventHorizon.Game.Server.Zone.Agent.Ai.State;
using EventHorizon.Plugin.Zone.Agent.Ai.Script;
using EventHorizon.TimerService;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace EventHorizon.Game.Server.Zone.Core
{
    public static class SystemAgentAiExtensions
    {
        public static void AddSystemAgentAi(this IServiceCollection services)
        {
            services
                .AddSingleton<IAgentRoutineRepository, AgentRoutineRepository>()
                .AddTransient<IScriptServices, ScriptServices>();
        }
        public static void UseSystemAgentAi(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetService<IMediator>().Publish(new LoadAgentRoutineSystemEvent()).GetAwaiter().GetResult();
            }
        }
    }
}