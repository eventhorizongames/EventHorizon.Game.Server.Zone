using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Interaction.Events;
using EventHorizon.Zone.System.Player.Events.ClientAction;
using MediatR;

namespace EventHorizon.Zone.System.Player.ClientAction
{
    public struct PlayerInteractionEventHandler : INotificationHandler<PlayerInteractionEvent>
    {
        readonly IMediator _mediator;
        public PlayerInteractionEventHandler(
            IMediator mediator
        )
        {
            _mediator = mediator;
        }
        public async Task Handle(
            PlayerInteractionEvent notification,
            CancellationToken cancellationToken
        )
        {
            await _mediator.Send(
                new RunInteractionCommand(
                    notification.InteractionEntityId,
                    notification.Player
                )
            );
        }
    }
}