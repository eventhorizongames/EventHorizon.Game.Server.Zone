using System;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Core.IdPool;
using EventHorizon.Game.Server.Zone.Entity.Model;
using EventHorizon.Game.Server.Zone.Entity.Register;
using EventHorizon.Game.Server.Zone.Entity.State;
using EventHorizon.Game.Server.Zone.Player.Actions;
using EventHorizon.Game.Server.Zone.Player.Actions.MovePlayer;
using EventHorizon.Game.Server.Zone.Player.Model;
using EventHorizon.Game.Server.Zone.Player.State;
using EventHorizon.Game.Server.Zone.Player.Zone;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Player.Connected.Handler
{
    public class PlayerConnectedHandler : INotificationHandler<PlayerConnectedEvent>
    {
        readonly IMediator _mediator;
        readonly IPlayerRepository _player;

        public PlayerConnectedHandler(IMediator mediator, IEntityRepository entityRepository, IPlayerRepository player)
        {
            _mediator = mediator;
            _player = player;
        }
        public async Task Handle(PlayerConnectedEvent notification, CancellationToken cancellationToken)
        {
            // Check for player on this zone server
            var player = await _player.FindById(notification.Id); 
            if (player.Equals(PlayerEntity.NULL))
            {
                var position = new PositionState
                {
                    CurrentPosition = Vector3.Zero,
                    NextMoveRequest = DateTime.Now.AddMilliseconds(MoveConstants.MOVE_DELAY_IN_MILLISECOND),
                    MoveToPosition = Vector3.Zero,
                };
                // Create new Player
                player = (Model.PlayerEntity)await _mediator.Send(new RegisterEntityEvent
                {
                    Entity = new Model.PlayerEntity(notification.Id, notification.ConnectionId, position)
                });
            }

            // Update players ConnectionId
            player.ConnectionId = notification.ConnectionId;
            await _player.Update(player);

            await _mediator.Send(new SendZoneInfoToPlayerEvent
            {
                Player = player
            });
        }
    }
}