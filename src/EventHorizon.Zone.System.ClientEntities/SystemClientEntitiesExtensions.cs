namespace EventHorizon.Game.Server.Zone
{
    using System.Collections.Generic;
    using System.Linq;
    using EventHorizon.Zone.System.ClientEntities.Load;
    using EventHorizon.Zone.System.ClientEntities.State;
    using MediatR;
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
        public static void UseSystemClientEntities(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var mediator = serviceScope.ServiceProvider
                    .GetService<IMediator>();
                mediator.Send(
                    new LoadSystemClientEntitiesCommand()
                ).GetAwaiter().GetResult();
            }
        }
    }
}