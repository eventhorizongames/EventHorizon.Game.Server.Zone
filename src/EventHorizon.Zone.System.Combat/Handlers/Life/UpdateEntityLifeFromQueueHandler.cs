namespace EventHorizon.Zone.System.Combat.Handlers.Life;

using EventHorizon.Zone.System.Combat.Events.Life;
using EventHorizon.Zone.System.Combat.Model.Life;
using EventHorizon.Zone.System.Combat.State;

using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

public class UpdateEntityLifeFromQueueHandler
    : INotificationHandler<UpdateEntityLifeFromQueueEvent>
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
