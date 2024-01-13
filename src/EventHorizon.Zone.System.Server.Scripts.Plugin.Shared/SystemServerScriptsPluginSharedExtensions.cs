namespace EventHorizon.Game.Server.Zone;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

public static class SystemServerScriptsPluginSharedExtensions
{
    public static IServiceCollection AddSystemServerScriptsPluginShared(
        this IServiceCollection services
    )
    {
        return services;
    }

    public static IApplicationBuilder UseSystemServerScriptsPluginShared(
        this IApplicationBuilder app
    )
    {
        return app;
    }
}
