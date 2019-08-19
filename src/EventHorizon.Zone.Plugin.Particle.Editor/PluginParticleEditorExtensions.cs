
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace EventHorizon.Game.Server.Zone
{
    public static class PluginParticleEditorExtensions
    {
        public static IServiceCollection AddPluginParticleEditor(
            this IServiceCollection services
        ) => services;

        public static IApplicationBuilder UsePluginParticleEditor(
            this IApplicationBuilder app
        ) => app;
    }
}