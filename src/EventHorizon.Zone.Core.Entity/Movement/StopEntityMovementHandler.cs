using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Events.Entity.Movement;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.Core.Model.Entity.State;
using MediatR;

namespace EventHorizon.Zone.Core.Entity.Movement
{
    public class StopEntityMovementHandler : INotificationHandler<StopEntityMovementEvent>
    {
        readonly EntityRepository _entityRepository;

        public StopEntityMovementHandler(
            EntityRepository entityRepository
        )
        {
            _entityRepository = entityRepository;
        }
        
        public async Task Handle(
            StopEntityMovementEvent notification,
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
            newPosition.CanMove = false;
            entity.Position = newPosition;
            await _entityRepository.Update(
                EntityAction.POSITION,
                entity
            );
        }
    }
}