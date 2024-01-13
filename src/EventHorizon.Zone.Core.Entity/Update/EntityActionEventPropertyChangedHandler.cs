namespace EventHorizon.Zone.Core.Entity.Update;

using System.Threading;
using System.Threading.Tasks;

using EventHorizon.Zone.Core.Events.Entity.Action;
using EventHorizon.Zone.Core.Events.Entity.Client;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.Core.Model.Entity.Client;

using MediatR;

public class EntityActionEventPropertyChangedHandler
    : INotificationHandler<EntityActionEvent>
{
    private readonly IMediator _mediator;

    public EntityActionEventPropertyChangedHandler(
        IMediator mediator
    )
    {
        _mediator = mediator;
    }

    public async Task Handle(
        EntityActionEvent notification,
        CancellationToken cancellationToken
    )
    {
        if (notification.Action.Equals(
            EntityAction.PROPERTY_CHANGED
        ) && notification.Entity is not null)
        {
            // Send Action to All Clients that Property Changed on Entity
            await _mediator.Publish(
                ClientActionEntityClientChangedToAllEvent.Create(
                    new EntityChangedData(
                        notification.Entity
                    )
                ),
                cancellationToken
            );
        }
    }
}
