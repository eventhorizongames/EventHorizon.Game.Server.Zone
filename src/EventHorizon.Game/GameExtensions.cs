namespace EventHorizon.Game
{
    using EventHorizon.Game.State;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

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