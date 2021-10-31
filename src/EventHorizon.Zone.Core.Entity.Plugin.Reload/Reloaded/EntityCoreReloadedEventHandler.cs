namespace EventHorizon.Zone.Core.Entity.Plugin.Reload.Reloaded
{
    using EventHorizon.Zone.Core.Entity.Plugin.Reload.ClientActions;
    using EventHorizon.Zone.Core.Events.Entity.Reload;

    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    public class EntityCoreReloadedEventHandler
        : INotificationHandler<EntityCoreReloadedEvent>
    {
        private readonly IMediator _mediator;

        public EntityCoreReloadedEventHandler
            (IMediator mediator
        )
        {
            _mediator = mediator;
        }

        public async Task Handle(
            EntityCoreReloadedEvent notification, 
            CancellationToken cancellationToken
        )
        {
            await _mediator.Publish(
                ClientActionEntityCoreReloadedToAllEvent.Create(
                    new (
                        notification.EntityConfiguration
                    )
                ),
                cancellationToken
            );
        }
    }
}
