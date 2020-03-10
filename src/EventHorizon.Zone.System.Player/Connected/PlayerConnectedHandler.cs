namespace EventHorizon.Zone.System.Player.Connected
{
    using global::System;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using EventHorizon.Zone.Core.Model.Player;
    using MediatR;
    using EventHorizon.Zone.Core.Events.Entity.Register;
    using EventHorizon.Zone.System.Player.Events.Connected;
    using EventHorizon.Zone.System.Player.Events.Details;
    using EventHorizon.Zone.System.Player.Events.Zone;
    using EventHorizon.Zone.Core.Model.ServerProperty;
    using EventHorizon.Zone.System.Player.Model.Action;
    using EventHorizon.Zone.System.Player.Mapper;
    using EventHorizon.Zone.Core.Events.Entity.Update;

    public class PlayerConnectedHandler : INotificationHandler<PlayerConnectedEvent>
    {
        readonly IMediator _mediator;
        readonly IServerProperty _serverProperty;
        readonly IPlayerRepository _player;

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
                    )
                );
                if (!globalPlayer.Position.CurrentZone.Equals(
                        _serverProperty.Get<string>(
                            ServerPropertyKeys.SERVER_ID
                        )
                    )
                )
                {
                    throw new Exception("Player is not part of this server.");
                }
                // Create new Player
                globalPlayer.Data["ConnectionId"] = notification.ConnectionId;
                player = (PlayerEntity)await _mediator.Send(
                    new RegisterEntityEvent
                    {
                        Entity = PlayerFromDetailsToEntity.MapToNew(
                            globalPlayer
                        ),
                    }
                );
                playerAction = StandardPlayerAction.REGISTERED;
            }

            // Update players ConnectionId
            player.ConnectionId = notification.ConnectionId;
            await _mediator.Send(
                new UpdateEntityCommand(
                    playerAction,
                    player
                )
            );

            await _mediator.Send(
                new SendZoneInfoToPlayerEvent(
                    player
                )
            );
        }
    }
}