namespace EventHorizon.Game.Remove
{
    using System.Threading;
    using System.Threading.Tasks;

    using EventHorizon.Game.Clear;
    using EventHorizon.Zone.Core.Events.Entity.Register;

    using MediatR;

    public class EntityUnRegisteredRemoveFromGameStateHandler : INotificationHandler<EntityUnRegisteredEvent>
    {
        private readonly IMediator _mediator;

        public EntityUnRegisteredRemoveFromGameStateHandler(
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
            await _mediator.Send(
                new ClearPlayerScore(
                    notification.EntityId
                )
            );
        }
    }
}
