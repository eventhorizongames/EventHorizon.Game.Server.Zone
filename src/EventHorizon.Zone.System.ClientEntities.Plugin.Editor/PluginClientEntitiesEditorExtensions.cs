
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace EventHorizon.Game.Server.Zone
{
    public static class PluginClientEntitiesEditorExtensions
    {
        public static IServiceCollection AddPluginClientEntitiesEditor(
            this IServiceCollection services
        ) => services;

        public static IApplicationBuilder UsePluginClientEntitiesEditor(
            this IApplicationBuilder app
        ) => app;
    }
}