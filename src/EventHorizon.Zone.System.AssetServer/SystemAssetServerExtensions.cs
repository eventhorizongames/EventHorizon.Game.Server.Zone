namespace EventHorizon.Game.Server.Zone;

using System;

using EventHorizon.Zone.System.AssetServer;
using EventHorizon.Zone.System.AssetServer.Model;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

public static class SystemAssetServerExtensions
{
    public static IServiceCollection AddSystemAssetServer(
        this IServiceCollection services,
        Action<SystemAssetServerOptions> options
    )
    {
        var scriptsOptions = new SystemAssetServerOptions();
        options(scriptsOptions);

        return services
            .AddSingleton<AssetServerSystemSettings>(
                new AssetServerSystemSettingsModel(
                    scriptsOptions
                )
            )
        ;
    }

    public static IApplicationBuilder UseSystemAssetServer(
        this IApplicationBuilder app
    )
    {
        return app;
    }
}
