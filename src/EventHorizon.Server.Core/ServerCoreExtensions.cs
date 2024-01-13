namespace EventHorizon.Server.Core;

using System;

using EventHorizon.Server.Core.Connection;
using EventHorizon.Server.Core.Connection.Internal;
using EventHorizon.Server.Core.Connection.Model;
using EventHorizon.Server.Core.Events.Register;
using EventHorizon.Server.Core.State;
using EventHorizon.Server.Core.Timer;
using EventHorizon.TimerService;

using MediatR;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

public static class ServerCoreExtensions
{
    public static IServiceCollection AddServerCore(
        this IServiceCollection services,
        Action<CoreSettings> configureCoreSettings
    ) => services
        .AddSingleton<ITimerTask, PingCoreServerTimerTask>()
        .AddSingleton<ITimerTask, CheckCoreServerConnectionTimerTask>()

        .AddSingleton<ServerCoreCheckState, SystemServerCoreCheckState>()

        .AddSingleton<CoreServerConnectionCache, SystemCoreServerConnectionCache>()
        .AddTransient<CoreServerConnectionFactory, SystemCoreServerConnectionFactory>()

        .Configure(
            configureCoreSettings
        )
    ;

    public static IApplicationBuilder UseServerCore(
        this IApplicationBuilder app
    )
    {
        using var serviceScope = app.CreateServiceScope();
        return app;
    }

    public static IApplicationBuilder UseRegisterWithCoreServer(
        this IApplicationBuilder app
    ) 
    {
        using var serviceScope = app.CreateServiceScope();
        serviceScope.ServiceProvider
            .GetRequiredService<IMediator>()
            .Publish(
                // TODO: Make this a Command/Request
                new RegisterWithCoreServer()
            ).GetAwaiter().GetResult();
        return app;
    }
}
