namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Run
{
    using EventHorizon.Zone.Core.Events.Entity.Find;
    using EventHorizon.Zone.Core.Model.DateTimeService;
    using EventHorizon.Zone.Core.Model.Entity;
    using EventHorizon.Zone.System.Agent.Plugin.Behavior.Api;
    using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;
    using EventHorizon.Zone.System.Agent.Plugin.Behavior.State;
    using EventHorizon.Zone.System.Agent.Plugin.Behavior.State.Queue;

    using global::System;
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    using Microsoft.Extensions.Logging;

    public class IsValidActorBehaviorTickHandler : IRequestHandler<IsValidActorBehaviorTick, ActorBehaviorTickValidationResponse>
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly ActorBehaviorTreeRepository _repository;
        private readonly IDateTimeService _dateTime;
        private readonly ActorBehaviorTickQueue _queue;

        public IsValidActorBehaviorTickHandler(
            ILogger<IsValidActorBehaviorTickHandler> logger,
            IMediator mediator,
            ActorBehaviorTreeRepository repository,
            IDateTimeService dateTime,
            ActorBehaviorTickQueue queue
        )
        {
            _logger = logger;
            _mediator = mediator;
            _repository = repository;
            _dateTime = dateTime;
            _queue = queue;
        }

        public async Task<ActorBehaviorTickValidationResponse> Handle(
            IsValidActorBehaviorTick request,
            CancellationToken cancellationToken
        )
        {
            var actor = await _mediator.Send(
                new GetEntityByIdEvent(
                    request.ActorBehaviorTick.ActorId
                )
            );
            if (actor == null || !actor.IsFound())
            {
                _logger.LogWarning(
                    "Actor was not Found \n | BehaviorTreeShapeId: {BehaviorTreeShapeId} \n | ActorId: {ActorId}",
                    request.ActorBehaviorTick.ShapeId,
                    request.ActorBehaviorTick.ActorId,
                    request
                );
                return new ActorBehaviorTickValidationResponse(
                    null,
                    default
                );
            }

            var agentBehavior = actor.GetProperty<AgentBehavior>(
                AgentBehavior.PROPERTY_NAME
            );
            if (
                agentBehavior.NextTickRequest.CompareTo(
                    _dateTime.Now
                ) >= 0
            )
            {
                RegisterActorWithBehaviorTreeForNextTickCycle(
                    request.ActorBehaviorTick
                );
                return new ActorBehaviorTickValidationResponse(
                    null,
                    default
                );
            }
            SetNextTickRequest(
                actor,
                agentBehavior,
                _dateTime.Now.AddSeconds(
                    15
                )
            );

            var shape = _repository.FindTreeShape(
                request.ActorBehaviorTick.ShapeId
            );
            if (!shape.IsValid || shape.NodeList.Count == 0)
            {
                _logger.LogWarning(
                    "Invalid or Empty Behavior Tree Shape \n | BehaviorTreeShapeId: {BehaviorTreeShapeId} \n | ActorId: {ActorId}",
                    request.ActorBehaviorTick.ShapeId,
                    request.ActorBehaviorTick.ActorId,
                    request
                );
                _queue.RegisterFailed(
                    request.ActorBehaviorTick
                );
                return new ActorBehaviorTickValidationResponse(
                    null,
                    default
                );
            }

            // Check to make sure that the request and Actor BehaviorTreeState match.
            var actorTreeState = actor.GetProperty<BehaviorTreeState>(
                BehaviorTreeState.PROPERTY_NAME
            );
            if (actorTreeState.IsValid && actorTreeState.ShapeId != shape.Id)
            {
                _logger.LogWarning(
                    "Pre Tick Not Matching Behavior Tree Shape \n | BehaviorTreeShapeId: {BehaviorTreeShapeId} \n | ActorId: {ActorId} \n | ActorBehaviorTreeShapeId: {ActorBehaviorTreeShapeId}",
                    request.ActorBehaviorTick.ShapeId,
                    request.ActorBehaviorTick.ActorId,
                    actorTreeState.ShapeId,
                    request
                );
                // If not the same, ignore this tick.
                // This might happen if the actor transitions to another behavior,
                //  between the queuing and the running of this shape.
                SetNextTickRequest(
                    actor,
                    agentBehavior,
                    _dateTime.Now.AddMilliseconds(
                        100
                    )
                );
                return new ActorBehaviorTickValidationResponse(
                    actor,
                    default
                );
            }

            return new ActorBehaviorTickValidationResponse(
                actor,
                shape
            );
        }

        private void RegisterActorWithBehaviorTreeForNextTickCycle(
            ActorBehaviorTick actorBehaviorTick
        )
        {
            _queue.Register(
                actorBehaviorTick.ShapeId,
                actorBehaviorTick.ActorId
            );
        }

        private IObjectEntity SetNextTickRequest(
            IObjectEntity actor,
            AgentBehavior agentBehavior,
            DateTime nextTickRequest
        )
        {
            agentBehavior.NextTickRequest = nextTickRequest;
            return actor.SetProperty(
                AgentBehavior.PROPERTY_NAME,
                agentBehavior
            );
        }
    }
}
