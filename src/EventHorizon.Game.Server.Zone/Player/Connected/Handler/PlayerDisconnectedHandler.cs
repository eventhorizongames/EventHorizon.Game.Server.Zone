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
    public class PlayerDisconnectedHandler : INotificationHandler<PlayerDisconnectedEvent>
    {
        readonly IMediator _mediator;
        readonly IPlayerRepository _player;

        public PlayerDisconnectedHandler(IMediator mediator, IPlayerRepository player)
        {
            _mediator = mediator;
            _player = player;
        }
        public async Task Handle(PlayerDisconnectedEvent notification, CancellationToken cancellationToken)
        {
            var player = await _player.FindById(notification.Id);
            if (player.IsFound())
            {
                await _mediator.Publish(new UnregisterEntityEvent
                {
                    Entity = player
                });
            }
        }
    }
}