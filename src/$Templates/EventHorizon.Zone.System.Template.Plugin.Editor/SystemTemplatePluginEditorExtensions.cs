namespace EventHorizon.Game.Server.Zone;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

public static class SystemTemplatePluginEditorExtensions
{
    public static IServiceCollection AddSystemTemplatePluginEditor(
        this IServiceCollection services
    ) => services;

    public static IApplicationBuilder UseSystemTemplatePluginEditor(
        this IApplicationBuilder app
    ) => app;
}
