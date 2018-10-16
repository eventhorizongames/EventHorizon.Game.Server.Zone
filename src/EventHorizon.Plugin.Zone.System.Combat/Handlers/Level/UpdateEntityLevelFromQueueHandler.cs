using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Plugin.Zone.System.Combat.Events.Level;
using EventHorizon.Plugin.Zone.System.Combat.Model.Level;
using EventHorizon.Plugin.Zone.System.Combat.State;
using MediatR;

namespace EventHorizon.Plugin.Zone.System.Combat.Handlers.Level
{
    public class UpdateEntityLevelFromQueueHandler : INotificationHandler<UpdateEntityLevelFromQueueEvent>
    {
        readonly IMediator _mediator;
        readonly IEntityQueue<UpdateEntityLevel> _entityQueue;

        public UpdateEntityLevelFromQueueHandler(
            IMediator mediator,
            IEntityQueue<UpdateEntityLevel> entityQueue)
        {
            _mediator = mediator;
            _entityQueue = entityQueue;
        }

        public async Task Handle(UpdateEntityLevelFromQueueEvent notification, CancellationToken cancellationToken)
        {
            var currentEntity = await _entityQueue.Dequeue();
            while (!currentEntity.Equals(UpdateEntityLevel.NULL))
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