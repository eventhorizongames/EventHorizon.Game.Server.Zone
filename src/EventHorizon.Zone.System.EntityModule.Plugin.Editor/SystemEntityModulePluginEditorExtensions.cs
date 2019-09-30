using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace EventHorizon.Game.Server.Zone
{
    public static class SystemEntityModulePluginEditorExtensions
    {
        public static IServiceCollection AddSystemEntityModulePluginEditor(
            this IServiceCollection services
        ) => services;

        public static IApplicationBuilder UseSystemEntityModulePluginEditor(
            this IApplicationBuilder app
        ) => app;
    }
}