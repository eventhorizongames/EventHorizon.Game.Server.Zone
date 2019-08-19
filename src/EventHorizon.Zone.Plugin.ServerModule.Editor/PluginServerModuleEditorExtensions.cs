
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace EventHorizon.Game.Server.Zone
{
    public static class PluginServerModuleEditorExtensions
    {
        public static IServiceCollection AddPluginServerModuleEditor(
            this IServiceCollection services
        ) => services;

        public static IApplicationBuilder UsePluginServerModuleEditor(
            this IApplicationBuilder app
        ) => app;
    }
}