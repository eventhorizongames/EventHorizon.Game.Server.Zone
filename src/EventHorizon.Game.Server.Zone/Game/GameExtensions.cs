namespace EventHorizon.Game.Server.Zone
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;
    using EventHorizon.Game.Server.Zone.Game.State;

    public static class GameExtensions
    {
        public static IServiceCollection AddGame(
            this IServiceCollection services
        ) => services
                .AddSingleton<GameState, InMemoryGameState>()
            ;

        public static IApplicationBuilder UseGame(
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