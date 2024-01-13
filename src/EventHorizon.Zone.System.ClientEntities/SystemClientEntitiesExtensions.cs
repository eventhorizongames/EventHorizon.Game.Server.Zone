namespace EventHorizon.Game.Server.Zone;

using EventHorizon.Zone.System.ClientEntities.Load;
using EventHorizon.Zone.System.ClientEntities.State;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

public static class SystemClientEntitiesExtensions
{
    public static IServiceCollection AddSystemClientEntities(
        this IServiceCollection services
    )
    {
        return services
            .AddSingleton<ClientEntityRepository, ClientEntityInMemoryRepository>()
        ;
    }
    public static void UseSystemClientEntities(
        this IApplicationBuilder app
    ) => app.SendMediatorCommand(
        new LoadSystemClientEntitiesCommand()
    );
}
