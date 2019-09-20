using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Entity.Model;
using EventHorizon.Game.Server.Zone.Entity.Registered;
using EventHorizon.Game.Server.Zone.Entity.State;
using EventHorizon.Zone.Core.Model.Entity;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Entity.Register.Handler
{
    public class RemoveSearchEntityOnUnregisteredHandler : INotificationHandler<EntityUnregisteredEvent>
    {
        readonly IMediator _mediator;
        readonly IEntitySearchTree _entitySearchTree;
        public RemoveSearchEntityOnUnregisteredHandler(IMediator mediator, IEntitySearchTree entitySearchTree)
        {
            _mediator = mediator;
            _entitySearchTree = entitySearchTree;
        }

        public Task Handle(EntityUnregisteredEvent notification, CancellationToken cancellationToken)
        {
            _entitySearchTree.Remove(new SearchEntity(notification.EntityId, Vector3.Zero, null));
            return Task.CompletedTask;
        }
    }
}