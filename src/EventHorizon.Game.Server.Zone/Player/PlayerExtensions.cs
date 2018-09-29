using EventHorizon.Game.Server.Core.Player.Connection;
using EventHorizon.Game.Server.Core.Player.Connection.Impl;
using EventHorizon.Game.Server.Core.Player.Connection.Impl.Testing;
using EventHorizon.Game.Server.Core.Player.Model;
using EventHorizon.Game.Server.Zone.Player.State;
using EventHorizon.Game.Server.Zone.Player.State.Impl;
using EventHorizon.Game.Server.Zone.Player.State.Impl.Testing;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventHorizon.Game.Server.Zone.Player
{
    public static class PlayerExtensions
    {
        public static IServiceCollection AddPlayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IPlayerRepository, PlayerRepository>()
                .Configure<PlayerSettings>(configuration.GetSection("Player"))
                .AddSingleton<IConnectionCache, ConnectionCache>()
                .AddTransient<IPlayerConnectionFactory, PlayerConnectionFactory>();

            if (configuration.GetValue<bool>("EnableTestingMode"))
            {
                services.AddTransient<IPlayerRepository, PlayerTestingRepository>()
                    .AddTransient<IPlayerConnectionFactory, PlayerTestingConnectionFactory>();
            }

            return services;
        }

        public static IApplicationBuilder UsePlayer(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {

            }
            return app;
        }
    }
}