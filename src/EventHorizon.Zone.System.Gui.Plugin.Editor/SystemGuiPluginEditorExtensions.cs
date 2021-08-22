namespace EventHorizon.Game.Server.Zone
{

    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

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
