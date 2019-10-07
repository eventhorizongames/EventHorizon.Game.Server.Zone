using EventHorizon.Zone.Core.Entity.State;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.Core.Model.Entity.State;
using Microsoft.Extensions.DependencyInjection;

namespace EventHorizon.Game.Server.Zone
{
    public static class CoreEntityExtensions
    {
        public static IServiceCollection AddCoreEntity(
            this IServiceCollection services
        )
        {
            return services
                .AddTransient<EntityRepository, InMemoryEntityRepository>()
                .AddTransient<EntitySearchTree, InMemoryEntitySearchTree>()
            ;
        }
        // public static IApplicationBuilder UseCoreEntity(
        //     this IApplicationBuilder app
        // )
        // {
        //     using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
        //     {
        //         return app;
        //     }
        // }
    }
}