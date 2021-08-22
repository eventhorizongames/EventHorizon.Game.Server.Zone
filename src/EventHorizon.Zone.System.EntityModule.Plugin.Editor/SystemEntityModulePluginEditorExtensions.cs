namespace EventHorizon.Game.Server.Zone
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

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
