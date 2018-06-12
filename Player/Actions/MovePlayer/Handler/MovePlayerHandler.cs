using System;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Core.Model;
using EventHorizon.Game.Server.Zone.Loop.Map;
using EventHorizon.Game.Server.Zone.Player.Client;
using MediatR;
using Microsoft.Extensions.Options;

namespace EventHorizon.Game.Server.Zone.Player.Actions.MovePlayer.Handler
{
    public class MovePlayerHandler : IRequestHandler<MovePlayerEvent, Vector3>
    {
        readonly IMediator _mediator;
        readonly ZoneSettings _zoneSettings;

        public MovePlayerHandler(IMediator mediator, IOptions<ZoneSettings> zoneSettings)
        {
            _mediator = mediator;
            _zoneSettings = zoneSettings.Value;
        }

        public async Task<Vector3> Handle(MovePlayerEvent request, CancellationToken cancellationToken)
        {
            var player = request.Player;
            if (player.Position.NextMoveRequest.CompareTo(DateTime.Now) >= 0)
            {
                return player.Position.MoveToPosition;
            }
            player.Position.CurrentPosition = player.Position.MoveToPosition;
            var direction = request.MoveDirection;
            var playerMapNode = await _mediator.Send(new GetMapNodeAtPositionEvent
            {
                Position = player.Position.CurrentPosition,
            });
            var movePlayerTo = player.Position.MoveToPosition;

            switch (direction)
            {
                case MoveDirections.Left:
                    movePlayerTo = Vector3.Add(playerMapNode.Position, new Vector3(-_zoneSettings.TileDimension, 0, 0));
                    break;
                case MoveDirections.Right:
                    movePlayerTo = Vector3.Add(playerMapNode.Position, new Vector3(_zoneSettings.TileDimension, 0, 0));
                    break;
                case MoveDirections.Backwards:
                    movePlayerTo = Vector3.Add(playerMapNode.Position, new Vector3(0, 0, -_zoneSettings.TileDimension));
                    break;
                case MoveDirections.Forward:
                    movePlayerTo = Vector3.Add(playerMapNode.Position, new Vector3(0, 0, _zoneSettings.TileDimension));
                    break;
                default:
                    movePlayerTo = player.Position.MoveToPosition;
                    break;
            }
            player.Position.MoveToPosition = movePlayerTo;
            player.Position.NextMoveRequest = DateTime.Now.AddMilliseconds(MoveConstants.MOVE_DELAY_IN_MILLISECOND);

            await _mediator.Publish(new ClientActionEvent
            {
                PlayerId = request.Player.Id,
                Action = "PlayerClientMove",
                Data = movePlayerTo,
            });

            return movePlayerTo;
        }
    }
}