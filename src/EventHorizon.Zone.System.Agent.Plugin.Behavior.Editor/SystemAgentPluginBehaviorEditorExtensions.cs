namespace EventHorizon.Game.Server.Zone
{
    using MediatR;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

    public static class SystemAgentPluginBehaviorEditorExtensions
    {
        public static IServiceCollection AddSystemAgentPluginBehaviorEditor(
            this IServiceCollection services
        ) => services;
        public static IApplicationBuilder UseSystemAgentPluginBehaviorEditor(
            this IApplicationBuilder app
        ) => app;
    }
}
