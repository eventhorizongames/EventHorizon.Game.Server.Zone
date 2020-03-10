namespace EventHorizon.Game.Server.Zone.Player.Move.Direction
{
    using System.Numerics;
    using System.Threading;
    using System.Threading.Tasks;
    using EventHorizon.Zone.Core.Events.Map;
    using EventHorizon.Zone.Core.Model.DateTimeService;
    using MediatR;
    using EventHorizon.Zone.Core.Model.Map;
    using EventHorizon.Game.Server.Zone.Player.Move.Model;
    using EventHorizon.Game.Server.Zone.Player.Action.Direction;
    using EventHorizon.Zone.Core.Events.Entity.Movement;

    public class MovePlayerHandler : INotificationHandler<MovePlayerEvent>
    {
        readonly IMediator _mediator;
        readonly IDateTimeService _dateTime;
        readonly IMapDetails _mapDetails;

        public MovePlayerHandler(
            IMediator mediator,
            IDateTimeService dateTime,
            IMapDetails mapDetails
        )
        {
            _mediator = mediator;
            _dateTime = dateTime;
            _mapDetails = mapDetails;
        }

        public async Task Handle(
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
                return;
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

            await _mediator.Send(
                new MoveEntityToPositionCommand(
                    player,
                    moveTo,
                    true
                )
            );
        }
    }
}