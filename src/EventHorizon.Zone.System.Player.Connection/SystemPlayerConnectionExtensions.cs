
using EventHorizon.Zone.System.Player.Connection;
using EventHorizon.Zone.System.Player.Connection.Internal;
using EventHorizon.Zone.System.Player.Connection.Model;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventHorizon.Game.Server.Zone
{
    public static class SystemPlayerConnectionExtensions
    {
        public static IServiceCollection AddSystemPlayerConnection(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            return services
                .AddSingleton<PlayerServerConnectionCache, SystemPlayerServerConnectionCache>()
                .AddTransient<PlayerServerConnectionFactory, SystemPlayerServerConnectionFactory>()
                .Configure<PlayerServerConnectionSettings>(
                    configuration.GetSection("Player")
                )
            ;
        }
        public static IApplicationBuilder UseSystemPlayerConnection(
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