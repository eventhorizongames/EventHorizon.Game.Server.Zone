namespace EventHorizon.Zone.Core.Entity.Movement
{
    using System.Threading;
    using System.Threading.Tasks;
    using EventHorizon.Zone.Core.Events.Entity.Movement;
    using EventHorizon.Zone.Core.Model.Core;
    using EventHorizon.Zone.Core.Model.Entity;
    using EventHorizon.Zone.Core.Model.Entity.State;
    using MediatR;

    public class EntityCanMoveHandler 
        : INotificationHandler<EntityCanMoveEvent>
    {
        private readonly EntityRepository _entityRepository;

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
            var locationState = entity.GetProperty<LocationState>(
                LocationState.PROPERTY_NAME
            );
            locationState.CanMove = true;
            entity.SetProperty(
                LocationState.PROPERTY_NAME,
                locationState
            );
            await _entityRepository.Update(
                EntityAction.PROPERTY_CHANGED,
                entity
            );
        }
    }
}