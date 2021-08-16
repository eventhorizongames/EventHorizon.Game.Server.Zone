namespace EventHorizon.Game.Server.Zone
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

    public static class SystemClientAssetsPluginEditorExtensions
    {
        public static IServiceCollection AddSystemClientAssetsPluginEditor(
            this IServiceCollection services
        ) => services;

        public static IApplicationBuilder UseSystemClientAssetsPluginEditor(
            this IApplicationBuilder app
        ) => app;
    }
}
