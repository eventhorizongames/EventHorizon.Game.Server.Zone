
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace EventHorizon.Game.Server.Zone
{
    public static class PluginClientAssetsEditorExtensions
    {
        public static IServiceCollection AddPluginClientAssetsEditor(
            this IServiceCollection services
        ) => services;
        
        public static IApplicationBuilder UsePluginClientAssetsEditor(
            this IApplicationBuilder app
        ) => app;
    }
}