using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Entity.Model;
using EventHorizon.Zone.Core.Entity.State;
using EventHorizon.Zone.Core.Events.Entity.Register;
using MediatR;

namespace EventHorizon.Zone.Core.Entity.Search
{
    public class RemoveSearchEntityOnUnregisteredHandler : INotificationHandler<EntityUnRegisteredEvent>
    {
        readonly EntitySearchTree _entitySearchTree;
        public RemoveSearchEntityOnUnregisteredHandler(
            EntitySearchTree entitySearchTree
        )
        {
            _entitySearchTree = entitySearchTree;
        }

        public Task Handle(
            EntityUnRegisteredEvent notification,
            CancellationToken cancellationToken
        )
        {
            _entitySearchTree.Remove(
                new SearchEntity(
                    notification.EntityId, 
                    Vector3.Zero, 
                    null
                )
            );
            return Task.CompletedTask;
        }
    }
}