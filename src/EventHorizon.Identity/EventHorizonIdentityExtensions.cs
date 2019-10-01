using EventHorizon.Identity.Client;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventHorizon.Game.Server.Zone
{
    public static class EventHorizonIdentityExtensions
    {
        public static IServiceCollection AddEventHorizonIdentity(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            return services
                .AddSingleton<ITokenClientFactory, CachingTokenClientFactory>()
            ;
        }
        public static IApplicationBuilder UseEventHorizonIdentity(
            this IApplicationBuilder app
        )
        {
            using (var serviceScope = app.CreateServiceScope())
            {
                return app;
            }
        }
    }
}