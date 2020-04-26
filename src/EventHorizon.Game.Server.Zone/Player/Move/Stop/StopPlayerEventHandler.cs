namespace EventHorizon.Game.Server.Zone.Player.Move.Stop
{
    using EventHorizon.Zone.Core.Events.Entity.Client;
    using EventHorizon.Zone.Core.Model.Entity.Client;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;

    public class StopPlayerEventHandler : INotificationHandler<StopPlayerEvent>
    {
        readonly IMediator _mediator;

        public StopPlayerEventHandler(
            IMediator mediator
        )
        {
            _mediator = mediator;
        }

        public Task Handle(
            StopPlayerEvent notification,
            CancellationToken cancellationToken
        )
        {
            return _mediator.Publish(
                ClientActionClientEntityStoppingToAllEvent.Create(
                    new EntityClientStoppingData
                    {
                        EntityId = notification.Player.Id
                    }
                )
            );
        }
    }
}