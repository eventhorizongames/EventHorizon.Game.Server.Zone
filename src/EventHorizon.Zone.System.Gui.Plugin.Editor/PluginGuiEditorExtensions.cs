
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace EventHorizon.Game.Server.Zone
{
    public static class PluginGuiEditorExtensions
    {
        public static IServiceCollection AddPluginGuiEditor(
            this IServiceCollection services
        ) => services;

        public static IApplicationBuilder UsePluginGuiEditor(
            this IApplicationBuilder app
        ) => app;
    }
}