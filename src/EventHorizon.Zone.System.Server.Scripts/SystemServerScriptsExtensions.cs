namespace EventHorizon.Game.Server.Zone;

using System;

using EventHorizon.Observer.Admin.State;
using EventHorizon.Observer.State;
using EventHorizon.Zone.System.Server.Scripts.Api;
using EventHorizon.Zone.System.Server.Scripts.Model;
using EventHorizon.Zone.System.Server.Scripts.State;
using EventHorizon.Zone.System.Server.Scripts.System;

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
            )
            .AddSingleton<ServerScriptsState, StandardServerScriptsState>()
            .AddSingleton<ServerScriptRepository, ServerScriptInMemoryRepository>()
            .AddSingleton<ServerScriptDetailsRepository, ServerScriptDetailsInMemoryRepository>()
            .AddSingleton<ServerScriptMediator, SystemServerScriptMediator>()
            .AddSingleton<ServerScriptObserverBroker, SystemServerScriptObserverBroker>()
            .AddTransient<ServerScriptServices, SystemServerScriptServices>()
            .AddSingleton<GenericObserverState>()
            .AddSingleton<ObserverState>(
                services => services.GetRequiredService<GenericObserverState>()
            )
            .AddSingleton<AdminObserverState>(
                services => services.GetRequiredService<GenericObserverState>()
            );
    }

    public static IApplicationBuilder UseSystemServerScripts(this IApplicationBuilder app)
    {
        return app;
    }
}
