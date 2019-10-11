using System;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Model.Player;
using EventHorizon.Game.Server.Zone.Player.Mapper;
using MediatR;
using EventHorizon.Zone.Core.Events.Entity.Register;
using EventHorizon.Zone.System.Player.Events.Connected;
using EventHorizon.Zone.System.Player.Events.Details;
using EventHorizon.Zone.System.Player.Events.Update;
using EventHorizon.Zone.System.Player.Events.Zone;
using EventHorizon.Zone.Core.Model.ServerProperty;
using EventHorizon.Zone.System.Player.Model.Action;

namespace EventHorizon.Game.Server.Zone.Player.Connected.Handler
{
    public class PlayerConnectedHandler : INotificationHandler<PlayerConnectedEvent>
    {
        readonly IServerProperty _serverProperty;
        readonly IMediator _mediator;
        readonly IPlayerRepository _player;

        public PlayerConnectedHandler(
            IServerProperty serverProperty,
            IMediator mediator,
            IPlayerRepository player
        )
        {
            _serverProperty = serverProperty;
            _mediator = mediator;
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
                    new PlayerGetDetailsEvent
                    {
                        Id = notification.Id
                    }
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
            await _player.Update(
                playerAction,
                player
            );
            await _mediator.Publish(
                new PlayerGlobalUpdateEvent(
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