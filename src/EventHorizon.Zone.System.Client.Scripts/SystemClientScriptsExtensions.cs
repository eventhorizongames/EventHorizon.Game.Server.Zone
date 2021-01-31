namespace EventHorizon.Game.Server.Zone
{
    using System;
    using EventHorizon.Zone.System.Client.Scripts.Api;
    using EventHorizon.Zone.System.Client.Scripts.Load;
    using EventHorizon.Zone.System.Client.Scripts.Model;
    using EventHorizon.Zone.System.Client.Scripts.State;
    using MediatR;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

    public static class SystemClientScriptsExtensions
    {
        public static IServiceCollection AddSystemClientScripts(
            this IServiceCollection services,
            Action<ClientScriptsOptions> options
        )
        {
            // Default settings
            var scriptsOptions = new ClientScriptsOptions
            {
                CompilerSubProcessDirectory = "/sub-processes/client-scripts",
                CompilerSubProcess = "EventHorizon.Game.Server.Zone.Client.Scripts.SubProcess",
            };
            options(scriptsOptions);

            return services
                .AddSingleton(
                    new ClientScriptsSettings(
                        scriptsOptions.CompilerSubProcessDirectory,
                        scriptsOptions.CompilerSubProcess
                    )
                ).AddSingleton<ClientScriptsState, InMemoryClientScriptsState>()
                .AddSingleton<ClientScriptRepository, ClientScriptInMemoryRepository>()
            ;
        }

        public static IApplicationBuilder UseSystemClientScripts(
            this IApplicationBuilder app
        )
        {
            using (var serviceScope = app.CreateServiceScope())
            {
                serviceScope.ServiceProvider
                    .GetService<IMediator>()
                    .Send(new LoadClientScriptsSystemCommand())
                    .GetAwaiter()
                    .GetResult();
            }
            return app;
        }
    }
}