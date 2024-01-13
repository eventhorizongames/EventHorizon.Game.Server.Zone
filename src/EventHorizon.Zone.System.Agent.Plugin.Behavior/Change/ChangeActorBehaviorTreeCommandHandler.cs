namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Change;

using EventHorizon.Zone.Core.Events.Entity.Update;
using EventHorizon.Zone.Core.Model.DateTimeService;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Api;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Register;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.State;

using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

public class ChangeActorBehaviorTreeCommandHandler : IRequestHandler<ChangeActorBehaviorTreeCommand, bool>
{
    private readonly IMediator _mediator;
    private readonly IDateTimeService _dateTime;
    private readonly ActorBehaviorTreeRepository _actorBehaviorTreeRepository;

    public ChangeActorBehaviorTreeCommandHandler(
        IMediator mediator,
        IDateTimeService dateTime,
        ActorBehaviorTreeRepository actorBehaviorTreeRepository
    )
    {
        _mediator = mediator;
        _dateTime = dateTime;
        _actorBehaviorTreeRepository = actorBehaviorTreeRepository;
    }

    public async Task<bool> Handle(
        ChangeActorBehaviorTreeCommand request,
        CancellationToken cancellationToken
    )
    {
        // Validate actor was found
        var actor = request.Entity;
        if (actor == null || !actor.IsFound())
        {
            return false;
        }

        // Find behavior tree
        var newBehaviorTreeShape = _actorBehaviorTreeRepository.FindTreeShape(
            request.NewBehaviorTreeShapeId
        );

        // Set actor behavior tree state to new shape, clearing it out.
        actor = actor.SetProperty(
            BehaviorTreeState.PROPERTY_NAME,
            new BehaviorTreeState(
                newBehaviorTreeShape
            )
        );

        // The actors behavior
        var agentBehavior = actor.GetProperty<AgentBehavior>(
            AgentBehavior.PROPERTY_NAME
        );

        // Update Actor Agent Behavior state
        agentBehavior.IsEnabled = true;
        agentBehavior.TreeId = request.NewBehaviorTreeShapeId;
        agentBehavior.NextTickRequest = _dateTime.Now.AddMilliseconds(
            100
        );
        actor = actor.SetProperty(
            AgentBehavior.PROPERTY_NAME,
            agentBehavior
        );
        await _mediator.Send(
            new UpdateEntityCommand(
                EntityAction.PROPERTY_CHANGED,
                actor
            )
        );

        await _mediator.Send(
            new RegisterActorWithBehaviorTreeForNextTickCycle(
                request.NewBehaviorTreeShapeId,
                actor.Id
            )
        );

        return true;
    }
}
