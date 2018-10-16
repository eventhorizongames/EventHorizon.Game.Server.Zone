using System;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Client;
using EventHorizon.Game.Server.Zone.Client.DataType;
using EventHorizon.Game.Server.Zone.Core.Model;
using EventHorizon.Game.Server.Zone.Entity.Model;
using EventHorizon.Game.Server.Zone.Load.Map;
using EventHorizon.Game.Server.Zone.Load.Map.Model;
using EventHorizon.Game.Server.Zone.Map;
using EventHorizon.Game.Server.Zone.Model.Structure;
using EventHorizon.Game.Server.Zone.Model.Core;
using EventHorizon.Game.Server.Zone.Player.Model;
using EventHorizon.Game.Server.Zone.Player.State;
using EventHorizon.Game.Server.Zone.Player.Update;
using MediatR;
using Microsoft.Extensions.Options;
using EventHorizon.Game.Server.Zone.Events.Map;
using EventHorizon.Game.Server.Zone.Events.Client.Actions;

namespace EventHorizon.Game.Server.Zone.Player.Actions.MovePlayer.Handler
{
    public class MovePlayerHandler : IRequestHandler<MovePlayerEvent, Vector3>
    {
        readonly IMediator _mediator;
        readonly ZoneMap _zoneMap;
        readonly IPlayerRepository _playerRepository;

        public MovePlayerHandler(IMediator mediator, IZoneMapFactory zoneMapFactory, IPlayerRepository playerRepository)
        {
            _mediator = mediator;
            _zoneMap = zoneMapFactory.Map;
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
                Position = currentPosition,
            });
            var moveTo = player.Position.MoveToPosition;

            switch (direction)
            {
                case MoveDirections.Left:
                    moveTo = Vector3.Add(playerMapNode.Position, new Vector3(-_zoneMap.TileDimensions, 0, 0));
                    break;
                case MoveDirections.Right:
                    moveTo = Vector3.Add(playerMapNode.Position, new Vector3(_zoneMap.TileDimensions, 0, 0));
                    break;
                case MoveDirections.Backwards:
                    moveTo = Vector3.Add(playerMapNode.Position, new Vector3(0, 0, -_zoneMap.TileDimensions));
                    break;
                case MoveDirections.Forward:
                    moveTo = Vector3.Add(playerMapNode.Position, new Vector3(0, 0, _zoneMap.TileDimensions));
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
                CurrentZone = player.Position.CurrentZone,
                ZoneTag = player.Position.ZoneTag,
            };
            await _playerRepository.Update(PlayerAction.POSITION, player);
            await _mediator.Publish(new PlayerGlobalUpdateEvent
            {
                Player = player,
            });

            await _mediator.Publish(new ClientActionEntityClientMoveEvent
            {
                Data = new EntityClientMoveData
                {
                    EntityId = player.Id,
                    MoveTo = moveTo
                },
            });

            return moveTo;
        }
    }
}