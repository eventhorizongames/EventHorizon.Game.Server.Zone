namespace EventHorizon.Zone.System.Player.Interaction.Action;

using EventHorizon.Zone.Core.Model.Id;
using EventHorizon.Zone.System.Player.Events.Interaction.Run;
using EventHorizon.Zone.System.Player.Plugin.Action.Events.Register;

using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

public class RegisterPlayerInteractionActionsHandler
    : INotificationHandler<ReadyForPlayerActionRegistration>
{
    private readonly IMediator _mediator;
    private readonly IdPool _idPool;

    public RegisterPlayerInteractionActionsHandler(
        IMediator mediator,
        IdPool idPool
    )
    {
        _mediator = mediator;
        _idPool = idPool;
    }

    public async Task Handle(
        ReadyForPlayerActionRegistration notification,
        CancellationToken cancellationToken
    )
    {
        // Move this Run into the Combat System
        await _mediator.Send(
            new RegisterPlayerAction(
                _idPool.NextId(),
                PlayerInteractionActions.INTERACT,
                new RunEntityInteractionActionEvent()
            )
        );
    }
}
