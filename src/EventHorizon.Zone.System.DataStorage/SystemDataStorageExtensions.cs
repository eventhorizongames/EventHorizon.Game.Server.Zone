namespace EventHorizon.Game.Server.Zone
{
    using EventHorizon.Zone.System.DataStorage.Model;
    using EventHorizon.Zone.System.DataStorage.Provider;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

    public static class SystemDataStorageExtensions
    {
        public static IServiceCollection AddSystemDataStorage(
            this IServiceCollection services
        )
        {
            return services
                .AddSingleton<DataStore, StandardDataStoreProvider>()
            ;
        }

        public static IApplicationBuilder UseSystemDataStorage(
            this IApplicationBuilder app
        )
        {
            return app;
        }
    }
}