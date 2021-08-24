namespace EventHorizon.Game.Server.Zone.Player.Move.Direction
{
    using System.Numerics;
    using System.Threading;
    using System.Threading.Tasks;

    using EventHorizon.Game.Server.Zone.Player.Action.Direction;
    using EventHorizon.Game.Server.Zone.Player.Move.Model;
    using EventHorizon.Zone.Core.Events.Entity.Movement;
    using EventHorizon.Zone.Core.Events.Map;
    using EventHorizon.Zone.Core.Model.Core;
    using EventHorizon.Zone.Core.Model.DateTimeService;
    using EventHorizon.Zone.Core.Model.Entity;
    using EventHorizon.Zone.Core.Model.Map;

    using MediatR;

    public class MovePlayerHandler
        : INotificationHandler<MovePlayerEvent>
    {
        private readonly IMediator _mediator;
        private readonly IDateTimeService _dateTime;
        private readonly IMapDetails _mapDetails;

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
            var transform = player.Transform;
            var locationState = player.GetProperty<LocationState>(
                LocationState.PROPERTY_NAME
            );
            // Player time to move has not expired or can is not set to Move, ignore request
            if (locationState.NextMoveRequest.CompareTo(_dateTime.Now) >= 0
                || !locationState.CanMove
            )
            {
                return;
            }

            var currentMoveToPosition = transform.Position;
            var direction = request.MoveDirection;
            var playerMapNode = await _mediator.Send(
                new GetMapNodeAtPositionEvent(
                    currentMoveToPosition
                ),
                cancellationToken
            );

            var moveTo = direction switch
            {
                MoveDirections.Left => Vector3.Add(
                    playerMapNode.Position,
                    new Vector3(
                        -_mapDetails.TileDimensions,
                        0,
                        0
                    )
                ),
                MoveDirections.Right => Vector3.Add(
                    playerMapNode.Position,
                    new Vector3(
                        _mapDetails.TileDimensions,
                        0,
                        0
                    )
                ),
                MoveDirections.Backwards => Vector3.Add(
                    playerMapNode.Position,
                     new Vector3(
                        0,
                        0,
                        -_mapDetails.TileDimensions
                    )
                ),
                MoveDirections.Forward => Vector3.Add(
                    playerMapNode.Position,
                    new Vector3(
                        0,
                        0,
                        _mapDetails.TileDimensions
                    )
                ),
                _ => transform.Position,
            };

            await _mediator.Send(
                new MoveEntityToPositionCommand(
                    player,
                    moveTo,
                    true
                ),
                cancellationToken
            );
        }
    }
}
