using EventHorizon.Game.Server.Zone.Entity.State;
using EventHorizon.Game.Server.Zone.Entity.State.Impl;
using Microsoft.Extensions.DependencyInjection;

namespace EventHorizon.Game.Server.Zone.Entity
{
    public static class EntityExtensions
    {
        public static void AddEntity(this IServiceCollection services)
        {
            services.AddTransient<IEntityRepository, EntityRepository>()
                .AddTransient<IEntitySearchTree, EntitySearchTree>();
        }
    }
}