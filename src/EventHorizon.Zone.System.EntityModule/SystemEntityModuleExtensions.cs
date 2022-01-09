namespace EventHorizon.Game.Server.Zone;

using EventHorizon.Zone.Core.Model.Command;
using EventHorizon.Zone.System.EntityModule.Api;
using EventHorizon.Zone.System.EntityModule.Load;
using EventHorizon.Zone.System.EntityModule.State;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

public static class SystemEntityModuleExtensions
{
    public static IServiceCollection AddSystemEntityModule(
        this IServiceCollection services
    ) => services
        .AddSingleton<EntityModuleRepository, EntityModuleInMemoryRepository>()
    ;

    public static IApplicationBuilder UseSystemEntityModule(
        this IApplicationBuilder app
    ) => app.SendMediatorCommand<LoadEntityModuleSystemCommand, StandardCommandResult>(
        new LoadEntityModuleSystemCommand()
    );
}
