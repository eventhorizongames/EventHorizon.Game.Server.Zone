namespace EventHorizon.Zone.System.Player.Interaction.Run
{
    using EventHorizon.Zone.System.Interaction.Events;
    using EventHorizon.Zone.System.Player.Events.Interaction.Run;

    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    public class RunEntityInteractionActionEventHandler
        : INotificationHandler<RunEntityInteractionActionEvent>
    {
        private readonly IMediator _mediator;

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
