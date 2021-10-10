namespace EventHorizon.Game.Server.Zone
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

    public static class SystemPlayerPluginEditorExtensions
    {
        public static IServiceCollection AddSystemPlayerPluginEditor(
            this IServiceCollection services
        ) => services;

        public static IApplicationBuilder UseSystemPlayerPluginEditor(
            this IApplicationBuilder app
        ) => app;
    }
}
