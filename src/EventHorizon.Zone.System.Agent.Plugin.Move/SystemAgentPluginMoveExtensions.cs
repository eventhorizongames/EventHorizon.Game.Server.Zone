using EventHorizon.Zone.System.Agent.Move.Timer;
using EventHorizon.Zone.System.Agent.Move.Repository;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using EventHorizon.Zone.System.Agent.Model.State;
using EventHorizon.TimerService;

namespace EventHorizon.Game.Server.Zone
{
    public static class SystemAgentPluginMoveExtensions
    {
        public static IServiceCollection AddSystemAgentPluginMove(
            this IServiceCollection services
        )
        {
            return services
                .AddTransient<IMoveAgentRepository, MoveAgentRepository>()
                .AddSingleton<ITimerTask, MoveRegisteredAgentsTimer>()
            ;
        }
        public static void UseSystemAgentPluginMove(
            this IApplicationBuilder app
        )
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
            }
        }
    }
}