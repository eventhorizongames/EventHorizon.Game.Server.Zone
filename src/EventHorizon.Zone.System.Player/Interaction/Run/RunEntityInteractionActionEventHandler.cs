using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Interaction.Events;
using EventHorizon.Zone.System.Player.Events.Interaction.Run;
using MediatR;

namespace EventHorizon.Zone.System.Player.Interaction.Run
{
    public struct RunEntityInteractionActionEventHandler : INotificationHandler<RunEntityInteractionActionEvent>
    {
        readonly IMediator _mediator;
        public RunEntityInteractionActionEventHandler(
            IMediator mediator
        )
        {
            _mediator = mediator;
        }
        public async Task Handle(
            RunEntityInteractionActionEvent notification,
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