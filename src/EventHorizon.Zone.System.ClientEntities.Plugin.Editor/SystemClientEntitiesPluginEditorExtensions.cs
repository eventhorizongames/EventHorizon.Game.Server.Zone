namespace EventHorizon.Game.Server.Zone
{

    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

    public static class SystemClientEntitiesPluginEditorExtensions
    {
        public static IServiceCollection AddSystemClientEntitiesPluginEditor(
            this IServiceCollection services
        ) => services;

        public static IApplicationBuilder UseSystemClientEntitiesPluginEditor(
            this IApplicationBuilder app
        ) => app;
    }
}
