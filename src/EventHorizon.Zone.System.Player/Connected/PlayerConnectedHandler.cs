namespace EventHorizon.Zone.System.Player.Connected;

using EventHorizon.Zone.Core.Events.Entity.Register;
using EventHorizon.Zone.Core.Events.Entity.Update;
using EventHorizon.Zone.Core.Model.Exceptions;
using EventHorizon.Zone.Core.Model.Player;
using EventHorizon.Zone.Core.Model.ServerProperty;
using EventHorizon.Zone.System.Player.Events.Connected;
using EventHorizon.Zone.System.Player.Events.Details;
using EventHorizon.Zone.System.Player.Events.Zone;
using EventHorizon.Zone.System.Player.Mapper;
using EventHorizon.Zone.System.Player.Model.Action;
using EventHorizon.Zone.System.Player.Model.Details;
using EventHorizon.Zone.System.Player.Set;

using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

public class PlayerConnectedHandler
    : INotificationHandler<PlayerConnectedEvent>
{
    private readonly IMediator _mediator;
    private readonly IServerProperty _serverProperty;
    private readonly IPlayerRepository _player;

    public PlayerConnectedHandler(
        IMediator mediator,
        IServerProperty serverProperty,
        IPlayerRepository player
    )
    {
        _mediator = mediator;
        _serverProperty = serverProperty;
        _player = player;
    }

    public async Task Handle(
        PlayerConnectedEvent notification,
        CancellationToken cancellationToken
    )
    {
        // Check for player on this zone server
        var playerAction = StandardPlayerAction.CONNECTION_ID;
        var player = await _player.FindById(
            notification.Id
        );
        if (!player.IsFound())
        {
            var globalPlayer = await _mediator.Send(
                new PlayerGetDetailsEvent(
                    notification.Id
                ),
                cancellationToken
            );
            if (!IsPlayerOnThisServer(
                globalPlayer
            ))
            {
                throw new PlatformErrorCodeException(
                    "player_not_part_of_server",
                    "Player is not part of this server."
                );
            }
            // Create new Player
            globalPlayer.Data["ConnectionId"] = notification.ConnectionId;
            var playerEntity = PlayerFromDetailsToEntity.MapToNew(
                globalPlayer
            );

            player = (PlayerEntity)await _mediator.Send(
                new RegisterEntityEvent(
                    playerEntity
                ),
                cancellationToken
            );
            playerAction = StandardPlayerAction.REGISTERED;
        }

        // Update players ConnectionId
        player.ConnectionId = notification.ConnectionId;
        await _mediator.Send(
            new UpdateEntityCommand(
                playerAction,
                player
            ),
            cancellationToken
        );

        await _mediator.Send(
            new SendZoneInfoToPlayerEvent(
                player
            ),
            cancellationToken
        );
    }

    private bool IsPlayerOnThisServer(
        PlayerDetails globalPlayer
    ) => !string.IsNullOrEmpty(globalPlayer.Location.CurrentZone)
        && globalPlayer.Location.CurrentZone.Equals(
            _serverProperty.Get<string>(
                ServerPropertyKeys.SERVER_ID
            )
        );
}
