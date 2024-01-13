namespace EventHorizon.Game.Server.Zone;

using EventHorizon.Zone.System.Player.Api;
using EventHorizon.Zone.System.Player.Load;
using EventHorizon.Zone.System.Player.State;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

public static class SystemPlayerExtensions
{
    public static IServiceCollection AddSystemPlayer(
        this IServiceCollection services
    ) => services
        .AddSingleton<PlayerSettingsState, InMemoryPlayerSettingsState>()
        .AddSingleton<PlayerSettingsCache>(
            services => services.GetRequiredService<PlayerSettingsState>()
        )
    ;

    public static IApplicationBuilder UseSystemPlayer(
        this IApplicationBuilder app
    ) => app.SendMediatorCommand<LoadPlayerSystemCommand, LoadPlayerSystemResult>(
        new LoadPlayerSystemCommand()
    );
}
