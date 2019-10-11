using EventHorizon.Zone.Core.Model.Player;
using EventHorizon.Game.Server.Zone.Player.State;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using EventHorizon.Zone.System.Player.Connection;
using EventHorizon.Game.Server.Core.Player.Connection.Testing;

namespace EventHorizon.Game.Server.Zone.Player
{
    public static class PlayerExtensions
    {
        public static IServiceCollection AddPlayer(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddTransient<IPlayerRepository, PlayerRepository>()
                .AddSystemPlayerConnection(
                    configuration
                )
            ;

            if (configuration.GetValue<bool>("EnableTestingMode"))
            {
                services.AddTransient<PlayerServerConnectionFactory, PlayerTestingConnectionFactory>();
            }

            return services;
        }

        public static IApplicationBuilder UsePlayer(this IApplicationBuilder app)
        {
            using (var serviceScope = app.CreateServiceScope())
            {
                return app;
            }
        }
    }
}