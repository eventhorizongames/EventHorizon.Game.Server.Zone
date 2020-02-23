using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Events.Client.Actions;
using EventHorizon.Zone.Core.Events.Entity.Action;
using EventHorizon.Zone.Core.Model.Client.DataType;
using EventHorizon.Zone.Core.Model.Entity;
using MediatR;

namespace EventHorizon.Zone.Core.Entity.Update
{
    public class EntityActionEventPropertyChangedHandler : INotificationHandler<EntityActionEvent>
    {
        private readonly IMediator _mediator;

        public EntityActionEventPropertyChangedHandler(IMediator mediator)
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
            ))
            {
                // Send Action to All Clients that Property Changed on Entity
                await _mediator.Publish(
                    new ClientActionEntityClientChangedToAllEvent
                    {
                        Data = new EntityChangedData(
                            notification.Entity
                        ),
                    }
                );
            }
        }
    }
}