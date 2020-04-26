namespace EventHorizon.Zone.Core.Entity.Update
{
    using EventHorizon.Zone.Core.Events.Entity.Action;
    using EventHorizon.Zone.Core.Events.Entity.Client;
    using EventHorizon.Zone.Core.Model.Entity;
    using EventHorizon.Zone.Core.Model.Entity.Client;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;

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
                    ClientActionEntityClientChangedToAllEvent.Create(
                        new EntityChangedData(
                            notification.Entity
                        )
                    )
                );
            }
        }
    }
}