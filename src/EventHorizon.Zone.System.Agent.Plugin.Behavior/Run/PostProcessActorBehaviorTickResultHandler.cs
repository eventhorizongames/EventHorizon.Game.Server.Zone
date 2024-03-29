namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Run;

using global::System;
using EventHorizon.Zone.Core.Model.DateTimeService;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.State;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.State.Queue;
using global::System.Threading;
using global::System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

public class PostProcessActorBehaviorTickResultHandler
    : IRequestHandler<PostProcessActorBehaviorTickResult>
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

    public Task Handle(
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
                "Kernel returned InValid Result \n | BehaviorTreeShapeId: {BehaviorTreeShapeId} \n | ActorId: {ActorId} \n | Request: {@BehaviorRequest}",
                request.ActorBehaviorTick.ShapeId,
                request.ActorBehaviorTick.ActorId,
                request
            );
            _queue.RegisterFailed(actorBehaviorTick);
            return Task.CompletedTask;
        }
        var actorTreeState = actor.GetProperty<BehaviorTreeState>(
            BehaviorTreeState.PROPERTY_NAME
        );
        if (!actorTreeState.IsValid)
        {
            actor = actor.SetProperty(
                BehaviorTreeState.PROPERTY_NAME,
                result.Report("SETTING PROPERTY on ACTOR")
            );
        }
        else if (behaviorTreeShape.IsValid && actorTreeState.ShapeId == behaviorTreeShape.Id)
        {
            actor = actor.SetProperty(
                BehaviorTreeState.PROPERTY_NAME,
                result.Report("SETTING PROPERTY on ACTOR")
            );
        }
        else
        {
            _logger.LogWarning(
                "Post Tick Not Matching Behavior Tree Shape \n | BehaviorTreeShapeId: {BehaviorTreeShapeId} \n | ActorId: {ActorId} \n | Report: {@BehaviorRequest}",
                request.ActorBehaviorTick.ShapeId,
                request.ActorBehaviorTick.ActorId,
                request
            );
        }
        var agentBehavior = actor.GetProperty<AgentBehavior>(AgentBehavior.PROPERTY_NAME);
        actor = SetNextTickRequest(
            actor,
            agentBehavior,
            _dateTime.Now.AddMilliseconds(
                100 // TODO: Get from settings
            )
        );
        _queue.Register(result.ShapeId, actor.Id);

        return Task.CompletedTask;
    }

    private IObjectEntity SetNextTickRequest(
        IObjectEntity actor,
        AgentBehavior agentBehavior,
        DateTime nextTickRequest
    )
    {
        agentBehavior.NextTickRequest = nextTickRequest;
        return actor.SetProperty(AgentBehavior.PROPERTY_NAME, agentBehavior);
    }
}
