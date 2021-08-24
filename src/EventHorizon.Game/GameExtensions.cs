namespace EventHorizon.Game
{
    using EventHorizon.Game.State;
    using EventHorizon.Game.Timer;
    using EventHorizon.TimerService;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

    public static class GameExtensions
    {
        public static IServiceCollection AddGame(
            this IServiceCollection services
        ) => services
            .AddSingleton<GameState, InMemoryGameState>()
            .AddSingleton<ITimerTask, RunPlayerListCaptureLogicTimerTask>()
        ;

        public static IApplicationBuilder UseGame(
            this IApplicationBuilder app
        ) => app;
    }
}
