using EventHorizon.Game.Server.Zone.Core.Ping;
using EventHorizon.Game.Server.Zone.Loop.Map;
using EventHorizon.Game.Server.Zone.Loop.State;
using EventHorizon.Game.Server.Zone.Loop.State.Impl;
using EventHorizon.Schedule;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventHorizon.Game.Server.Zone.Loop
{
    public static class LoopExtensions
    {
        public static void AddLoop(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IServerState, ServerState>();
            services.AddSingleton<IScheduledTask, PingCoreServerScheduledTask>();
        }
        public static void UseLoop(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetService<IMediator>().Publish(new CreateMapEvent()).GetAwaiter().GetResult();
            }
        }
    }
}