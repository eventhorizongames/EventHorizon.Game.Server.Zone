namespace EventHorizon.Game.Server.Zone
{
    using EventHorizon.TimerService;
    using EventHorizon.Zone.System.ClientAssets.Load;
    using EventHorizon.Zone.System.ClientAssets.State;
    using EventHorizon.Zone.System.ClientAssets.State.Api;
    using EventHorizon.Zone.System.ClientAssets.Timer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

    public static class SystemClientAssetsExtensions
    {
        public static IServiceCollection AddSystemClientAssets(
            this IServiceCollection services
        ) => services
            //.AddSingleton<ITimerTask, SaveClientAssetsTimerTask>()

            .AddSingleton<ClientAssetRepository, ClientAssetInMemoryRepository>()
        ;

        public static IApplicationBuilder UseSystemClientAssets(
            this IApplicationBuilder app
        ) => app.SendMediatorCommand(
            new LoadSystemClientAssetsCommand()
        );
    }
}