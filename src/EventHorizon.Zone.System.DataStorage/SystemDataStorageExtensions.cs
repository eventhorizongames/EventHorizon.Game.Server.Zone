namespace EventHorizon.Game.Server.Zone
{
    using EventHorizon.TimerService;
    using EventHorizon.Zone.System.DataStorage.Api;
    using EventHorizon.Zone.System.DataStorage.Load;
    using EventHorizon.Zone.System.DataStorage.Model;
    using EventHorizon.Zone.System.DataStorage.Provider;
    using EventHorizon.Zone.System.DataStorage.Timer;

    using MediatR;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

    public static class SystemDataStorageExtensions
    {
        public static IServiceCollection AddSystemDataStorage(
            this IServiceCollection services
        )
        {
            return services
                .AddSingleton<StandardDataStoreProvider>()
                .AddSingleton<DataStore>(
                    services => services.GetRequiredService<StandardDataStoreProvider>()
                ).AddSingleton<DataStoreManagement>(
                    services => services.GetRequiredService<StandardDataStoreProvider>()
                ).AddSingleton<ITimerTask, SaveDataStoreTimerTask>()
            ;
        }

        public static IApplicationBuilder UseSystemDataStorage(
            this IApplicationBuilder app
        )
        {
            using var serviceScope = app.CreateServiceScope();
            serviceScope.ServiceProvider
                .GetService<IMediator>()
                .Send(
                    new LoadDataStoreCommand()
                ).GetAwaiter()
                .GetResult();
            return app;
        }
    }
}
