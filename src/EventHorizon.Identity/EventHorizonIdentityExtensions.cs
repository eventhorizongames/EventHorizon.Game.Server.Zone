using System;

using EventHorizon.Identity.Client;
using EventHorizon.Identity.Model;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace EventHorizon.Identity
{
    public static class EventHorizonIdentityExtensions
    {
        public static IServiceCollection AddEventHorizonIdentity(
            this IServiceCollection services,
            Action<AuthSettings> configureAuthSettings
        ) => services
            .AddSingleton<ITokenClientFactory, CachingTokenClientFactory>()
            .Configure<AuthSettings>(
                configureAuthSettings
            )
        ;

        public static IApplicationBuilder UseEventHorizonIdentity(
            this IApplicationBuilder app
        )
        {
            using (var serviceScope = app.CreateServiceScope())
            {
            }
            return app;
        }
    }
}
