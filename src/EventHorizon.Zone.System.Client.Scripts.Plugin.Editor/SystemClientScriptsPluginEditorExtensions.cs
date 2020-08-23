namespace EventHorizon.Game.Server.Zone
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

    public static class SystemClientScriptsPluginEditorExtensions
    {
        public static IServiceCollection AddSystemClientScriptsPluginEditor(
            this IServiceCollection services
        ) => services;

        public static IApplicationBuilder UseSystemClientScriptsPluginEditor(
            this IApplicationBuilder app
        ) => app;
    }
}