using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Plugin.Zone.System.Combat.Events.Life;
using EventHorizon.Plugin.Zone.System.Combat.Model.Life;
using EventHorizon.Plugin.Zone.System.Combat.State;
using MediatR;

namespace EventHorizon.Plugin.Zone.System.Combat.Handlers.Life
{
    public class UpdateEntityLifeFromQueueHandler : INotificationHandler<UpdateEntityLifeFromQueueEvent>
    {
        readonly IMediator _mediator;
        readonly IEntityQueue<ChangeEntityLife> _entityQueue;

        public UpdateEntityLifeFromQueueHandler(
            IMediator mediator,
            IEntityQueue<ChangeEntityLife> entityQueue)
        {
            _mediator = mediator;
            _entityQueue = entityQueue;
        }

        public async Task Handle(UpdateEntityLifeFromQueueEvent notification, CancellationToken cancellationToken)
        {
            var currentEntity = await _entityQueue.Dequeue();
            while (!currentEntity.Equals(ChangeEntityLife.NULL))
            {
                await _mediator.Publish(
                    new RunEntityLifeStateChangeEvent
                    {
                        EntityId = currentEntity.EntityId,
                        Property = currentEntity.Property,
                        Points = currentEntity.Points,
                    }
                );
                currentEntity = await _entityQueue.Dequeue();
            }
        }
    }
}