namespace EventHorizon.Zone.Core.Entity.Register
{
    using EventHorizon.Zone.Core.Events.Entity.Client;
    using EventHorizon.Zone.Core.Events.Entity.Register;
    using EventHorizon.Zone.Core.Model.Entity.Client;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;

    public class EntityUnregisteredHandler : INotificationHandler<EntityUnRegisteredEvent>
    {
        readonly IMediator _mediator;
        public EntityUnregisteredHandler(
            IMediator mediator
        )
        {
            _mediator = mediator;
        }

        public async Task Handle(
            EntityUnRegisteredEvent notification,
            CancellationToken cancellationToken
        )
        {
            await _mediator.Publish(
                ClientActionEntityUnregisteredToAllEvent.Create(
                    new EntityUnregisteredData
                    {
                        EntityId = notification.EntityId,
                    }
                )
            );
        }
    }
}