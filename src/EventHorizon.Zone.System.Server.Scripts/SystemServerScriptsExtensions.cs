namespace EventHorizon.Game.Server.Zone
{
    using System;
    using EventHorizon.Zone.System.Server.Scripts.Api;
    using EventHorizon.Zone.System.Server.Scripts.Load;
    using EventHorizon.Zone.System.Server.Scripts.Model;
    using EventHorizon.Zone.System.Server.Scripts.State;
    using EventHorizon.Zone.System.Server.Scripts.System;
    using MediatR;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

    public static class SystemServerScriptsExtensions
    {
        public static IServiceCollection AddSystemServerScripts(
            this IServiceCollection services,
            Action<ServerScriptsOptions> options
        )
        {
            // Default settings
            var scriptsOptions = new ServerScriptsOptions
            {
                CompilerSubProcessDirectory = "/sub-processes/server-scripts",
                CompilerSubProcess = "EventHorizon.Game.Server.Zone.Server.Scripts.SubProcess",
            };
            options(scriptsOptions);

            return services
                .AddSingleton(
                    new ServerScriptsSettings(
                        scriptsOptions.CompilerSubProcessDirectory,
                        scriptsOptions.CompilerSubProcess
                    )
                ).AddSingleton<ServerScriptsState, StandardServerScriptsState>()
                .AddSingleton<ServerScriptRepository, ServerScriptInMemoryRepository>()
                .AddSingleton<ServerScriptDetailsRepository, ServerScriptDetailsInMemoryRepository>()
                .AddTransient<ServerScriptServices, SystemServerScriptServices>()
            ;
        }
        
        public static IApplicationBuilder UseSystemServerScripts(
            this IApplicationBuilder app
        )
        {
            return app;
        }
    }
}