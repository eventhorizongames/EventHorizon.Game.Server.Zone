using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Plugin.Zone.System.Combat.Events.Level;
using EventHorizon.Plugin.Zone.System.Combat.Model.Level;
using EventHorizon.Plugin.Zone.System.Combat.State;
using MediatR;

namespace EventHorizon.Plugin.Zone.System.Combat.Handlers.Level
{
    public class EntityLevelUpFromQueueHandler : INotificationHandler<EntityLevelUpFromQueueEvent>
    {
        readonly IMediator _mediator;
        readonly IEntityQueue<EntityLevelUp> _entityQueue;

        public EntityLevelUpFromQueueHandler(
            IMediator mediator,
            IEntityQueue<EntityLevelUp> entityQueue)
        {
            _mediator = mediator;
            _entityQueue = entityQueue;
        }

        public async Task Handle(EntityLevelUpFromQueueEvent notification, CancellationToken cancellationToken)
        {
            var currentEntity = await _entityQueue.Dequeue();
            while (!currentEntity.Equals(EntityLevelUp.NULL))
            {
                await _mediator.Publish(
                    new RunLevelUpForEntityEvent
                    {
                        EntityId = currentEntity.EntityId,
                        Property = currentEntity.Property
                    }
                );
                currentEntity = await _entityQueue.Dequeue();
            }
        }
    }
}