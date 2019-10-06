using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Events.Entity.Movement;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.Core.Model.Entity.State;
using MediatR;

namespace EventHorizon.Zone.Core.Entity.Movement
{
    public class EntityCanMoveHandler : INotificationHandler<EntityCanMoveEvent>
    {
        readonly EntityRepository _entityRepository;
        public EntityCanMoveHandler(
            EntityRepository entityRepository
        )
        {
            _entityRepository = entityRepository;
        }
        public async Task Handle(
            EntityCanMoveEvent notification, 
            CancellationToken cancellationToken
        )
        {
            var entity = await _entityRepository.FindById(
                notification.EntityId
            );
            if (!entity.IsFound())
            {
                return;
            }
            var newPosition = entity.Position;
            newPosition.CanMove = true;
            entity.Position = newPosition;
            await _entityRepository.Update(
                EntityAction.POSITION, 
                entity
            );
        }
    }
}