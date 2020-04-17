namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Run
{
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using EventHorizon.Zone.Core.Model.Entity;
    using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;
    using EventHorizon.Zone.System.Agent.Plugin.Behavior.State;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using EventHorizon.Zone.Core.Model.DateTimeService;
    using global::System;
    using EventHorizon.Zone.System.Agent.Plugin.Behavior.State.Queue;

    public class PostProcessActorBehaviorTickResultHandler : IRequestHandler<PostProcessActorBehaviorTickResult>
    {
        private readonly ILogger _logger;
        private readonly IDateTimeService _dateTime;
        private readonly ActorBehaviorTickQueue _queue;

        public PostProcessActorBehaviorTickResultHandler(
            ILogger<PostProcessActorBehaviorTickResultHandler> logger,
            IDateTimeService dateTime,
            ActorBehaviorTickQueue queue
        )
        {
            _logger = logger;
            _dateTime = dateTime;
            _queue = queue;
        }

        public Task<Unit> Handle(
            PostProcessActorBehaviorTickResult request,
            CancellationToken cancellationToken
        )
        {
            var result = request.Result;
            var actorBehaviorTick = request.ActorBehaviorTick;
            var actor = request.Actor;
            var behaviorTreeShape = request.Shape;

            if (!result.IsValid)
            {
                _logger.LogWarning(
                    "Kernel returned InValid Result \n | BehaviorTreeShapeId: {BehaviorTreeShapeId} \n | ActorId: {ActorId}",
                    request.ActorBehaviorTick.ShapeId,
                    request.ActorBehaviorTick.ActorId,
                    request
                );
                _queue.RegisterFailed(
                    request.ActorBehaviorTick
                );
                return Unit.Task;
            }
            var actorTreeState = actor.GetProperty<BehaviorTreeState>(
                BehaviorTreeState.PROPERTY_NAME
            );
            if (!actorTreeState.IsValid)
            {
                actor = actor.SetProperty(
                    BehaviorTreeState.PROPERTY_NAME,
                    result.Report(
                        "SETTING PROPERTY on ACTOR"
                    )
                );
            }
            else if (behaviorTreeShape.IsValid && actorTreeState.ShapeId == behaviorTreeShape.Id)
            {
                actor = actor.SetProperty(
                    BehaviorTreeState.PROPERTY_NAME,
                    result.Report(
                        "SETTING PROPERTY on ACTOR"
                    )
                );
            }
            else
            {
                _logger.LogWarning(
                    "Post Tick Not Matching Behavior Tree Shape \n | BehaviorTreeShapeId: {BehaviorTreeShapeId} \n | ActorId: {ActorId}",
                    request.ActorBehaviorTick.ShapeId,
                    request.ActorBehaviorTick.ActorId,
                    request
                );
            }
            var agentBehavior = actor.GetProperty<AgentBehavior>(
                AgentBehavior.PROPERTY_NAME
            );
            actor = SetNextTickRequest(
                actor,
                agentBehavior,
                _dateTime.Now.AddMilliseconds(
                    100
                )
            );
            _queue.Register(
                result.ShapeId,
                actor.Id
            );

            return Unit.Task;
        }

        private IObjectEntity SetNextTickRequest(
            IObjectEntity actor,
            AgentBehavior agentBehavior,
            DateTime nextTickRequest
        )
        {
            agentBehavior.NextTickRequest = nextTickRequest;
            return actor.SetProperty<AgentBehavior>(
                AgentBehavior.PROPERTY_NAME,
                agentBehavior
            );
        }
    }
}