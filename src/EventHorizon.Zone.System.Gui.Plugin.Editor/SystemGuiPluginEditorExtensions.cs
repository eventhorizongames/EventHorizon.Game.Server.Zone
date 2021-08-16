
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace EventHorizon.Game.Server.Zone
{
    public static class SystemGuiPluginEditorExtensions
    {
        public static IServiceCollection AddSystemGuiPluginEditor(
            this IServiceCollection services
        ) => services;

        public static IApplicationBuilder UseSystemGuiPluginEditor(
            this IApplicationBuilder app
        ) => app;
    }
}
