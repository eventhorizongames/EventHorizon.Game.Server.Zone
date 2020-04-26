namespace EventHorizon.Zone.Core.Entity.Movement
{
    using EventHorizon.Zone.Core.Events.Entity.Client;
    using EventHorizon.Zone.Core.Events.Entity.Movement;
    using EventHorizon.Zone.Core.Events.Entity.Update;
    using EventHorizon.Zone.Core.Events.Map;
    using EventHorizon.Zone.Core.Model.Core;
    using EventHorizon.Zone.Core.Model.DateTimeService;
    using EventHorizon.Zone.Core.Model.Entity;
    using EventHorizon.Zone.Core.Model.Entity.Client;
    using EventHorizon.Zone.Core.Model.Entity.Movement;
    using EventHorizon.Zone.Core.Model.Settings;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;

    public class MoveEntityToPositionCommandHandler : IRequestHandler<MoveEntityToPositionCommand, MoveEntityToPositionCommandResponse>
    {
        private readonly IMediator _mediator;
        private readonly IDateTimeService _dateTimeService;
        private readonly ZoneSettings _zoneSettings;

        public MoveEntityToPositionCommandHandler(
            IMediator mediator,
            IDateTimeService dateTimeService,
            ZoneSettings zoneSettings
        )
        {
            _mediator = mediator;
            _dateTimeService = dateTimeService;
            _zoneSettings = zoneSettings;
        }

        public async Task<MoveEntityToPositionCommandResponse> Handle(
            MoveEntityToPositionCommand request,
            CancellationToken cancellationToken
        )
        {
            if (request.DoDensityCheck)
            {
                // Check for Dense playerMoveToMapNode
                var moveToMapNode = await _mediator.Send(
                    new GetMapNodeAtPositionEvent
                    {
                        Position = request.MoveTo,
                    }
                );
                if (moveToMapNode.Info.ContainsKey("dense")
                    && (int)moveToMapNode.Info["dense"] > 0
                )
                {
                    return new MoveEntityToPositionCommandResponse(
                        false,
                        "move_to_node_is_dense"
                    );
                }
            }
            var entity = request.Entity;
            var transform = entity.Transform;
            var locationState = entity.GetProperty<LocationState>(
                LocationState.PROPERTY_NAME
            );
            transform.Position = request.MoveTo;
            locationState.MoveToPosition = request.MoveTo;
            locationState.NextMoveRequest = _dateTimeService.Now.AddMilliseconds(
                (int)(_zoneSettings.BaseMovementTimeOffset * (1 / this.GetMovementSpeedMultiplier(
                    entity
                )))
            );
            // Set Transform
            entity.Transform = transform;
            // Set Property LocationState
            entity.SetProperty(
                LocationState.PROPERTY_NAME,
                locationState
            );

            // Update Entity Command
            await _mediator.Send(
                new UpdateEntityCommand(
                    EntityAction.POSITION,
                    entity
                )
            );

            // Send update to all clients that an entity moved to a new Position
            await _mediator.Publish(
                ClientActionEntityClientMoveToAllEvent.Create(
                    new EntityClientMoveData
                    {
                        EntityId = entity.Id,
                        MoveTo = request.MoveTo
                    }
                )
            );

            return new MoveEntityToPositionCommandResponse(
                true
            );
        }

        private float GetMovementSpeedMultiplier(
            IObjectEntity entity
        )
        {
            // TODO: Read MovementState to get SpeedMultiplier
            return entity.GetProperty<MovementState>(
                MovementState.PROPERTY_NAME
            ).Speed;
        }
    }
}