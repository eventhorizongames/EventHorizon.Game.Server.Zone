using EventHorizon.Game.Server.Zone.Core.Ping;
using EventHorizon.Game.Server.Zone.Map.Create;
using EventHorizon.Game.Server.Zone.State;
using EventHorizon.Game.Server.Zone.State.Impl;
using EventHorizon.Schedule;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventHorizon.Game.Server.Zone.Setup
{
    public static class ServerSetupExtensions
    {
        public static void AddServerSetup(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IServerState, ServerState>()
                .AddSingleton<IScheduledTask, PingCoreServerScheduledTask>();
        }
        public static void UseSetupServer(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetService<IMediator>().Publish(new CreateMapEvent()).GetAwaiter().GetResult();
            }
        }
    }
}