using System;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Client;
using EventHorizon.Game.Server.Zone.Core.Model;
using EventHorizon.Game.Server.Zone.Entity.Model;
using EventHorizon.Game.Server.Zone.Loop.Map;
using EventHorizon.Game.Server.Zone.Player.State;
using MediatR;
using Microsoft.Extensions.Options;

namespace EventHorizon.Game.Server.Zone.Player.Actions.MovePlayer.Handler
{
    public class MovePlayerHandler : IRequestHandler<MovePlayerEvent, Vector3>
    {
        readonly IMediator _mediator;
        readonly ZoneSettings _zoneSettings;
        readonly IPlayerRepository _playerRepository;

        public MovePlayerHandler(IMediator mediator, IOptions<ZoneSettings> zoneSettings, IPlayerRepository playerRepository)
        {
            _mediator = mediator;
            _zoneSettings = zoneSettings.Value;
            _playerRepository = playerRepository;
        }

        public async Task<Vector3> Handle(MovePlayerEvent request, CancellationToken cancellationToken)
        {
            var player = request.Player;
            if (player.Position.NextMoveRequest.CompareTo(DateTime.Now) >= 0)
            {
                return player.Position.MoveToPosition;
            }
            var currentPosition = player.Position.MoveToPosition;
            var direction = request.MoveDirection;
            var playerMapNode = await _mediator.Send(new GetMapNodeAtPositionEvent
            {
                Position = player.Position.MoveToPosition,
            });
            var moveTo = player.Position.MoveToPosition;

            switch (direction)
            {
                case MoveDirections.Left:
                    moveTo = Vector3.Add(playerMapNode.Position, new Vector3(-_zoneSettings.TileDimension, 0, 0));
                    break;
                case MoveDirections.Right:
                    moveTo = Vector3.Add(playerMapNode.Position, new Vector3(_zoneSettings.TileDimension, 0, 0));
                    break;
                case MoveDirections.Backwards:
                    moveTo = Vector3.Add(playerMapNode.Position, new Vector3(0, 0, -_zoneSettings.TileDimension));
                    break;
                case MoveDirections.Forward:
                    moveTo = Vector3.Add(playerMapNode.Position, new Vector3(0, 0, _zoneSettings.TileDimension));
                    break;
                default:
                    moveTo = player.Position.MoveToPosition;
                    break;
            }
            player.Position = new PositionState
            {
                CurrentPosition = player.Position.MoveToPosition,
                MoveToPosition = moveTo,
                NextMoveRequest = DateTime.Now.AddMilliseconds(MoveConstants.MOVE_DELAY_IN_MILLISECOND),
            };
            await _playerRepository.Update(player);

            await _mediator.Publish(new ClientActionEvent
            {
                Action = "EntityClientMove",
                Data = new
                {
                    entityId = request.Player.Id,
                    moveTo
                },
            });

            return moveTo;
        }
    }
}