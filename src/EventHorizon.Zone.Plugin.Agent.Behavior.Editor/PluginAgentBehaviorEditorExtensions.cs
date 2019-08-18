using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace EventHorizon.Game.Server.Zone
{
    public static class PluginAgentBehaviorEditorExtensions
    {
        public static IServiceCollection AddPluginAgentBehaviorEditor(
            this IServiceCollection services
        ) => services;
        public static IApplicationBuilder UsePluginAgentBehaviorEditor(
            this IApplicationBuilder app
        ) => app;
    }
}