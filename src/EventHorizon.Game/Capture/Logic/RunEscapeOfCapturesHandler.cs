namespace EventHorizon.Game.Capture.Logic
{
    using System.Threading;
    using System.Threading.Tasks;
    using EventHorizon.Game.Model;
    using EventHorizon.Zone.Core.Events.Entity.Update;
    using EventHorizon.Zone.Core.Model.Entity;
    using EventHorizon.Zone.System.Agent.Events.Get;
    using EventHorizon.Zone.System.Agent.Plugin.Companion.Model;
    using EventHorizon.Zone.System.Combat.Events.Skill.Runner;
    using MediatR;

    public class RunEscapeOfCapturesHandler : IRequestHandler<RunEscapeOfCaptures>
    {
        private IMediator _mediator;

        public RunEscapeOfCapturesHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(
            RunEscapeOfCaptures request, 
            CancellationToken cancellationToken
        )
        {
            var playerEntity = request.PlayerEntity;
            var captureState = playerEntity.GetProperty<GamePlayerCaptureState>(
                GamePlayerCaptureState.PROPERTY_NAME
            );
            foreach (var globalId in captureState.CompanionsCaught)
            {
                var companionEntity = await _mediator.Send(
                    new FindAgentByIdEvent(
                        globalId
                    ),
                    cancellationToken
                );
                var ownerState = companionEntity.GetProperty<OwnerState>(
                    OwnerState.PROPERTY_NAME
                );
                ownerState.OwnerId = string.Empty;
                companionEntity = companionEntity.SetProperty(
                    OwnerState.PROPERTY_NAME,
                    ownerState
                );
                await _mediator.Send(
                    new UpdateEntityCommand(
                        EntityAction.PROPERTY_CHANGED,
                        companionEntity
                    ),
                    cancellationToken
                );
            }
            await _mediator.Publish(
                new RunSkillWithTargetOfEntityEvent
                {
                    ConnectionId = playerEntity.ConnectionId,
                    // TODO: This will change when the Skill is implemented.
                    SkillId = "skill_id",
                    CasterId = playerEntity.Id,
                    TargetId = playerEntity.Id,
                },
                cancellationToken
            );

            return Unit.Value;
        }
    }
}
