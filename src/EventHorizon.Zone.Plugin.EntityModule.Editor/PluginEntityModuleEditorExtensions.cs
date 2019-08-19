using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace EventHorizon.Game.Server.Zone
{
    public static class PluginEntityModuleEditorExtensions
    {
        public static IServiceCollection AddPluginEntityModuleEditor(
            this IServiceCollection services
        ) => services;

        public static IApplicationBuilder UsePluginEntityModuleEditor(
            this IApplicationBuilder app
        ) => app;
    }
}