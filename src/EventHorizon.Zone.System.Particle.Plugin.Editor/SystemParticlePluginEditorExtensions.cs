namespace EventHorizon.Game.Server.Zone;


using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

public static class SystemParticlePluginEditorExtensions
{
    public static IServiceCollection AddSystemParticlePluginEditor(
        this IServiceCollection services
    ) => services;

    public static IApplicationBuilder UseSystemParticlePluginEditor(
        this IApplicationBuilder app
    ) => app;
}
