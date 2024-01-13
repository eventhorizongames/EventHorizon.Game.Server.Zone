namespace EventHorizon.Zone.System.Combat.Life.Property;

using EventHorizon.Zone.System.Combat.Events.Life;
using EventHorizon.Zone.System.Combat.Model.Life;
using EventHorizon.Zone.System.Combat.State;

using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

public class DecreaseLifePropertyHandler
    : INotificationHandler<DecreaseLifePropertyEvent>
{
    readonly IEntityQueue<ChangeEntityLife> _entityQueue;
    public DecreaseLifePropertyHandler(
        IEntityQueue<ChangeEntityLife> entityQueue
    )
    {
        _entityQueue = entityQueue;
    }
    public async Task Handle(DecreaseLifePropertyEvent notification, CancellationToken cancellationToken)
    {
        await _entityQueue.Enqueue(new ChangeEntityLife
        {
            EntityId = notification.EntityId,
            Property = notification.Property,
            Points = -notification.Points,
        });
    }
}
