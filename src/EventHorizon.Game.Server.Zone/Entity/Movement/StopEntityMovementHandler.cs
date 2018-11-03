using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Events.Entity.Movement;
using EventHorizon.Game.Server.Zone.External.Entity;
using EventHorizon.Game.Server.Zone.Model.Entity;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Entity.Movement
{
    public class StopEntityMovementHandler : INotificationHandler<StopEntityMovementEvent>
    {
        readonly IEntityRepository _entityRepository;
        public StopEntityMovementHandler(
            IEntityRepository entityRepository
        )
        {
            _entityRepository = entityRepository;
        }
        public async Task Handle(StopEntityMovementEvent notification, CancellationToken cancellationToken)
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
            await _entityRepository.Update(EntityAction.POSITION, entity);
        }
    }
}