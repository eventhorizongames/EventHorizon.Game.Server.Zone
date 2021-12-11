namespace EventHorizon.Game.Server.Zone;

using System;

using EventHorizon.Zone.System.ArtifactManagement;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

public static class SystemArtifactManagementExtensions
{
    public static IServiceCollection AddSystemArtifactManagement(
        this IServiceCollection services,
        Action<SystemArtifactManagementOptions> options
    )
    {
        var scriptsOptions = new SystemArtifactManagementOptions();
        options?.Invoke(scriptsOptions);

        return services
            .AddSingleton<ArtifactManagementSystemSettings>(
                new ArtifactManagementSystemSettingsModel(
                    scriptsOptions
                )
            )
        ;
    }

    public static IApplicationBuilder UseSystemArtifactManagement(
        this IApplicationBuilder app
    ) => app;
}
