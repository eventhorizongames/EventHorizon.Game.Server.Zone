namespace EventHorizon.Zone.System.Combat.Handlers.Level;

using EventHorizon.Zone.System.Combat.Events.Level;
using EventHorizon.Zone.System.Combat.Model.Level;
using EventHorizon.Zone.System.Combat.State;

using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

public class EntityLevelUpFromQueueHandler
    : INotificationHandler<EntityLevelUpFromQueueEvent>
{
    private readonly IMediator _mediator;
    private readonly IEntityQueue<EntityLevelUp> _entityQueue;

    public EntityLevelUpFromQueueHandler(
        IMediator mediator,
        IEntityQueue<EntityLevelUp> entityQueue
    )
    {
        _mediator = mediator;
        _entityQueue = entityQueue;
    }

    public async Task Handle(
        EntityLevelUpFromQueueEvent notification,
        CancellationToken cancellationToken
    )
    {
        var currentEntity = await _entityQueue.Dequeue();
        while (!currentEntity.Equals(
            EntityLevelUp.NULL
        ))
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
