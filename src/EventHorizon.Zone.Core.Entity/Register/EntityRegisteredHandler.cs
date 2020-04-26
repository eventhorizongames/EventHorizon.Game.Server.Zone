namespace EventHorizon.Zone.Core.Entity.Register
{
    using EventHorizon.Zone.Core.Events.Entity.Client;
    using EventHorizon.Zone.Core.Events.Entity.Register;
    using EventHorizon.Zone.Core.Model.Entity.Client;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;

    public class EntityRegisteredHandler : INotificationHandler<EntityRegisteredEvent>
    {
        readonly IMediator _mediator;
        public EntityRegisteredHandler(
            IMediator mediator
        )
        {
            _mediator = mediator;
        }

        public async Task Handle(
            EntityRegisteredEvent notification,
            CancellationToken cancellationToken
        )
        {
            await _mediator.Publish(
                ClientActionEntityRegisteredToAllEvent.Create(
                    new EntityRegisteredData
                    {
                        Entity = notification.Entity,
                    }
                )
            );
        }
    }
}