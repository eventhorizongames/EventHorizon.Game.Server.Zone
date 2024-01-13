namespace EventHorizon.Game.Server.Zone.Player;

using EventHorizon.Game.Server.Core.Player.Connection.Testing;
using EventHorizon.Game.Server.Zone.Player.State;
using EventHorizon.Zone.Core.Model.Player;
using EventHorizon.Zone.System.Player.Connection;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class PlayerExtensions
{
    public static IServiceCollection AddServerPlayer(
        this IServiceCollection services,
        IConfiguration configuration
    )
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

    public static IApplicationBuilder UseServerPlayer(
        this IApplicationBuilder app
    )
    {
        using (var serviceScope = app.CreateServiceScope())
        {
        }
        return app;
    }
}
