
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace EventHorizon.Game.Server.Zone
{
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