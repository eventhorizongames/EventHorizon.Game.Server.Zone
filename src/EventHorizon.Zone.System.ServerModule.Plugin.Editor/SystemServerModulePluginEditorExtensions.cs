namespace EventHorizon.Game.Server.Zone
{

    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

    public static class SystemServerModulePluginEditorExtensions
    {
        public static IServiceCollection AddSystemServerModulePluginEditor(
            this IServiceCollection services
        ) => services;

        public static IApplicationBuilder UseSystemServerModulePluginEditor(
            this IApplicationBuilder app
        ) => app;
    }
}
