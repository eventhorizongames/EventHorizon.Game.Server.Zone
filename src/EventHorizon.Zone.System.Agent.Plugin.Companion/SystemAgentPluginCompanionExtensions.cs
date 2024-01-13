namespace EventHorizon.Game.Server.Zone;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

public static class SystemAgentPluginCompanionExtensions
{
    public static IServiceCollection AddSystemAgentPluginCompanion(
        this IServiceCollection services
    )
    {
        return services;
    }

    public static IApplicationBuilder UseSystemAgentPluginCompanion(
        this IApplicationBuilder app
    )
    {
        return app;
    }
}
