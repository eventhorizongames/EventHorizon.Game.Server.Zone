namespace EventHorizon.Zone.System.Player.Connected;

using EventHorizon.Zone.Core.Events.Entity.Register;
using EventHorizon.Zone.Core.Model.Player;
using EventHorizon.Zone.System.Player.Events.Connected;

using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

public class PlayerDisconnectedHandler
    : INotificationHandler<PlayerDisconnectedEvent>
{
    private readonly IMediator _mediator;
    private readonly IPlayerRepository _player;

    public PlayerDisconnectedHandler(
        IMediator mediator,
        IPlayerRepository player
    )
    {
        _mediator = mediator;
        _player = player;
    }

    public async Task Handle(
        PlayerDisconnectedEvent notification,
        CancellationToken cancellationToken
    )
    {
        var player = await _player.FindById(notification.Id);
        if (player.IsFound())
        {
            await _mediator.Publish(
                new UnRegisterEntityEvent(
                    player
                )
            );
        }
    }
}
