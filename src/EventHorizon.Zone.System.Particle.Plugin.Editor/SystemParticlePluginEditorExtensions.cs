
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace EventHorizon.Game.Server.Zone
{
    public static class SystemParticlePluginEditorExtensions
    {
        public static IServiceCollection AddSystemParticlePluginEditor(
            this IServiceCollection services
        ) => services;

        public static IApplicationBuilder UseSystemParticlePluginEditor(
            this IApplicationBuilder app
        ) => app;
    }
}
