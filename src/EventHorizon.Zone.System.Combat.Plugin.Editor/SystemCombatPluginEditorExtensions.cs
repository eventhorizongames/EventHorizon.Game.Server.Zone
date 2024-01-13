namespace EventHorizon.Game.Server.Zone.Core;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

public static class SystemCombatPluginEditorExtensions
{
    public static IServiceCollection AddSystemCombatPluginEditor(
        this IServiceCollection services
    )
    {
        return services;
    }
    public static IApplicationBuilder UseSystemCombatPluginEditor(
        this IApplicationBuilder app
    )
    {
        return app;
    }
}
