using EventHorizon.Game.Server.Zone.Core.Register;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace EventHorizon.Game.Server.Zone.Core
{
    public static class CoreExtensions
    {
        public static IServiceCollection AddServerCore(
            this IServiceCollection services
        ) => services;

        public static void UseServerCore(
            this IApplicationBuilder app
        )
        {
            using (var serviceScope = app.CreateServiceScope())
            {
                serviceScope.ServiceProvider
                    .GetService<IMediator>()
                    .Publish(
                        new RegisterWithCoreServerEvent()
                    ).GetAwaiter().GetResult();
            }
        }
    }
}