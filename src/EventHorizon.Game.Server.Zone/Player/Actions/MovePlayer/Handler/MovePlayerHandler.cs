using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Model.Client.DataType;
using EventHorizon.Zone.Core.Events.Client.Actions;
using EventHorizon.Zone.Core.Events.Map;
using EventHorizon.Zone.Core.Model.DateTimeService;
using EventHorizon.Zone.Core.Model.Player;
using EventHorizon.Game.Server.Zone.Player.Model;
using EventHorizon.Game.Server.Zone.Player.Update;
using MediatR;
using EventHorizon.Zone.Core.Model.Map;
using EventHorizon.Zone.System.Player.Events.Update;

namespace EventHorizon.Game.Server.Zone.Player.Actions.MovePlayer.Handler
{
    public class MovePlayerHandler : IRequestHandler<MovePlayerEvent, Vector3>
    {
        readonly IMediator _mediator;
        readonly IDateTimeService _dateTime;
        readonly IMapDetails _mapDetails;
        readonly IPlayerRepository _playerRepository;

        public MovePlayerHandler(
            IMediator mediator,
            IDateTimeService dateTime,
            IMapDetails mapDetails,
            IPlayerRepository playerRepository
        )
        {
            _mediator = mediator;
            _dateTime = dateTime;
            _mapDetails = mapDetails;
            _playerRepository = playerRepository;
        }

        public async Task<Vector3> Handle(
            MovePlayerEvent request,
            CancellationToken cancellationToken
        )
        {
            var player = request.Player;
            // Player time to move has not expired or can is not set to Move, ignore request
            if (player.Position.NextMoveRequest.CompareTo(_dateTime.Now) >= 0
                || !player.Position.CanMove
            )
            {
                return player.Position.MoveToPosition;
            }
            var currentPosition = player.Position.MoveToPosition;
            var direction = request.MoveDirection;
            var playerMapNode = await _mediator.Send(
                new GetMapNodeAtPositionEvent
                {
                    Position = currentPosition,
                }
            );
            var moveTo = player.Position.MoveToPosition;

            switch (direction)
            {
                case MoveDirections.Left:
                    moveTo = Vector3.Add(
                        playerMapNode.Position,
                        new Vector3(
                            -_mapDetails.TileDimensions,
                            0,
                            0
                        )
                    );
                    break;
                case MoveDirections.Right:
                    moveTo = Vector3.Add(
                        playerMapNode.Position,
                        new Vector3(
                            _mapDetails.TileDimensions,
                            0,
                            0
                        )
                    );
                    break;
                case MoveDirections.Backwards:
                    moveTo = Vector3.Add(
                        playerMapNode.Position,
                        new Vector3(
                            0,
                            0,
                            -_mapDetails.TileDimensions
                        )
                    );
                    break;
                case MoveDirections.Forward:
                    moveTo = Vector3.Add(
                        playerMapNode.Position,
                        new Vector3(
                            0,
                            0,
                            _mapDetails.TileDimensions
                        )
                    );
                    break;
                default:
                    moveTo = player.Position.MoveToPosition;
                    break;
            }
            // Check for Dense playerMoveToMapNode
            var playerMoveToMapNode = await _mediator.Send(
                new GetMapNodeAtPositionEvent
                {
                    Position = moveTo,
                }
            );
            if (playerMoveToMapNode.Info.ContainsKey("dense")
                && (int)playerMoveToMapNode.Info["dense"] > 0
            )
            {
                // TODO: Send message to player they cannot move to position, it is a wall ;)
                // await _mediator.Publish(new ClientActionEntityClientMoveToAllEvent
                // {
                //     Data = new EntityClientMoveData
                //     {
                //         EntityId = player.Id,
                //         MoveTo = moveTo
                //     },
                // });
                return moveTo;
            }
            var newPosition = player.Position;
            newPosition.CurrentPosition = player.Position.MoveToPosition;
            newPosition.MoveToPosition = moveTo;
            newPosition.NextMoveRequest = _dateTime.Now.AddMilliseconds(
                MoveConstants.MOVE_DELAY_IN_MILLISECOND
            );
            player.Position = newPosition;
            await _playerRepository.Update(
                PlayerAction.POSITION,
                player
            );
            await _mediator.Publish(
                new PlayerGlobalUpdateEvent(
                    player
                )
            );

            await _mediator.Publish(
                new ClientActionEntityClientMoveToAllEvent
                {
                    Data = new EntityClientMoveData
                    {
                        EntityId = player.Id,
                        MoveTo = moveTo
                    },
                }
            );

            return moveTo;
        }
    }
}