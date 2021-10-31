namespace EventHorizon.Zone.Core.Entity.Plugin.Reload.Reloaded
{
    using EventHorizon.Zone.Core.Entity.Plugin.Reload.ClientActions;
    using EventHorizon.Zone.Core.Events.Entity.Reload;

    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    public class CoreEntityReloadedEventHandler
        : INotificationHandler<CoreEntityReloadedEvent>
    {
        private readonly IMediator _mediator;

        public CoreEntityReloadedEventHandler
            (IMediator mediator
        )
        {
            _mediator = mediator;
        }

        public async Task Handle(
            CoreEntityReloadedEvent notification, 
            CancellationToken cancellationToken
        )
        {
            await _mediator.Publish(
                ClientActionCoreEntityReloadedToAllEvent.Create(
                    new (
                        notification.EntityConfiguration
                    )
                ),
                cancellationToken
            );
        }
    }
}
