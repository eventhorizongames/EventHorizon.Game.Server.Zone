namespace EventHorizon.Game.Server.Zone
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

    public static class SystemAgentPluginEditorExtensions
    {
        public static IServiceCollection AddSystemAgentPluginEditor(
            this IServiceCollection services
        ) => services;

        public static IApplicationBuilder UseSystemAgentPluginEditor(
            this IApplicationBuilder app
        ) => app;
    }
}