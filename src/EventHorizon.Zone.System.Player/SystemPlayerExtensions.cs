namespace EventHorizon.Game.Server.Zone
{
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
            .AddSingleton<PlayerConfigurationState, InMemoryPlayerConfigurationState>()
            .AddSingleton<PlayerConfigurationCache>(
                services => services.GetRequiredService<PlayerConfigurationState>()
            )
        ;

        public static IApplicationBuilder UseSystemPlayer(
            this IApplicationBuilder app
        ) => app.SendMediatorCommand<LoadSystemPlayerCommand, LoadSystemPlayerResult>(
            new LoadSystemPlayerCommand()
        );
    }
}
