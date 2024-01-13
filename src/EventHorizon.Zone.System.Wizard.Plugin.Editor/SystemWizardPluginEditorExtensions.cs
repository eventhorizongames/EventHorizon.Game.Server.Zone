namespace EventHorizon.Game.Server.Zone;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

public static class SystemWizardPluginEditorExtensions
{
    public static IServiceCollection AddSystemWizardPluginEditor(
        this IServiceCollection services
    ) => services
    ;

    public static IApplicationBuilder UseSystemWizardPluginEditor(
        this IApplicationBuilder app
    )
    {
        return app;
    }
}
