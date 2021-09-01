namespace EventHorizon.Game.Server.Zone
{
    using EventHorizon.Zone.System.Server.Scripts.Plugin.BackgroundTask.Api;
    using EventHorizon.Zone.System.Server.Scripts.Plugin.BackgroundTask.Builders;
    using EventHorizon.Zone.System.Server.Scripts.Plugin.BackgroundTask.State;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

    public static class SystemServerScriptsPluginBackgroundTaskExtensions
    {
        public static IServiceCollection AddSystemServerScriptsPluginBackgroundTask(
            this IServiceCollection services
        ) => services
            .AddSingleton<BackgroundTaskWrapperRepository, InMemoryBackgroundTaskWrapperRepository>()
            .AddSingleton<BackgroundTaskWrapperBuilder, ThreadedBackgroundTaskWrapperBuilder>()
        ;

        public static IApplicationBuilder UseSystemServerScriptsPluginBackgroundTask(
            this IApplicationBuilder app
        )
        {
            return app;
        }
    }
}
