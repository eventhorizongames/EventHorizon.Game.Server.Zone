namespace EventHorizon.Game.Server.Zone
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

    public static class SystemClientScriptsPluginSharedExtensions
    {
        public static IServiceCollection AddSystemClientScriptsPluginShared(
            this IServiceCollection services
        )
        {
            return services;
        }

        public static IApplicationBuilder UseSystemClientScriptsPluginShared(
            this IApplicationBuilder app
        )
        {
            return app;
        }
    }
}
