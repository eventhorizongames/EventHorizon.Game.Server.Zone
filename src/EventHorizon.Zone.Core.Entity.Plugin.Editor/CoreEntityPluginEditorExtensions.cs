namespace EventHorizon.Game.Server.Zone;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

public static class CoreEntityPluginEditorExtensions
{
    public static IServiceCollection AddCoreEntityPluginEditor(
        this IServiceCollection services
    ) => services;

    public static IApplicationBuilder UseCoreEntityPluginEditor(
        this IApplicationBuilder app
    ) => app;
}
