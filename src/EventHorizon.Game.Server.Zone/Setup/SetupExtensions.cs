using EventHorizon.Game.Server.Zone.Server.Core.Ping.Tasks;
using EventHorizon.Schedule;
using EventHorizon.Zone.Core.Events.Map.Create;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventHorizon.Game.Server.Zone.Setup
{
    public static class ServerSetupExtensions
    {
        public static void AddServerSetup(
            this IServiceCollection services, 
            IConfiguration configuration
        )
        {
            services
                .AddSingleton<IScheduledTask, PingCoreServerScheduledTask>();
        }

        public static void UseSetupServer(
            this IApplicationBuilder app
        )
        {
            using (var serviceScope = app.CreateServiceScope())
            {
                serviceScope.ServiceProvider
                    .GetService<IMediator>()
                    .Publish(
                        new CreateMapEvent()
                    ).GetAwaiter().GetResult();
            }
        }
    }
}