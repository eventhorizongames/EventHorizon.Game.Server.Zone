using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace EventHorizon.Game.Server.Zone
{
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