using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Events.Entity.Movement;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.Core.Model.Entity;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Entity.Movement
{
    public class EntityCanMoveHandler : INotificationHandler<EntityCanMoveEvent>
    {
        readonly IEntityRepository _entityRepository;
        public EntityCanMoveHandler(
            IEntityRepository entityRepository
        )
        {
            _entityRepository = entityRepository;
        }
        public async Task Handle(EntityCanMoveEvent notification, CancellationToken cancellationToken)
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
            await _entityRepository.Update(EntityAction.POSITION, entity);
        }
    }
}