using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Player.State;
using EventHorizon.Game.Server.Zone.Player.Zone;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Player.Connected.Handler
{
    public class PlayerConnectedHandler : INotificationHandler<PlayerConnectedEvent>
    {
        readonly IMediator _mediator;
        readonly IPlayerRepository _player;
        public PlayerConnectedHandler(IMediator mediator, IPlayerRepository player)
        {
            _mediator = mediator;
            _player = player;
        }
        public async Task Handle(PlayerConnectedEvent notification, CancellationToken cancellationToken)
        {
            var player = await _player.FindById(notification.Id);
            if (player == null)
            {
                // Create new Player
                player = new Model.PlayerEntity
                {
                    Id = notification.Id,
                };
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