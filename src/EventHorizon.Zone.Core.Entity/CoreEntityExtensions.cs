namespace EventHorizon.Game.Server.Zone;

using EventHorizon.Zone.Core.Entity.Api;
using EventHorizon.Zone.Core.Entity.Load;
using EventHorizon.Zone.Core.Entity.State;
using EventHorizon.Zone.Core.Model.Entity.State;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

public static class CoreEntityExtensions
{
    public static IServiceCollection AddCoreEntity(
        this IServiceCollection services
    ) => services
        .AddTransient<EntityRepository, InMemoryEntityRepository>()
        .AddTransient<EntitySearchTree, InMemoryEntitySearchTree>()
        .AddSingleton<EntitySettingsState, InMemoryEntitySettingsState>()
        .AddSingleton<EntitySettingsCache>(
            services => services.GetRequiredService<EntitySettingsState>()
        )
    ;

    public static IApplicationBuilder UseCoreEntity(
        this IApplicationBuilder app
    ) => app.SendMediatorCommand<LoadEntityCoreCommand, LoadEntityCoreResult>(
        new LoadEntityCoreCommand()
    );
}
