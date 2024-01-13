namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Load;

using global::System.Threading;
using global::System.Threading.Tasks;
using MediatR;

public class LoadAgentBehaviorSystemHandler : IRequestHandler<LoadAgentBehaviorSystem>
{
    private readonly IMediator _mediator;

    public LoadAgentBehaviorSystemHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Handle(
        LoadAgentBehaviorSystem request,
        CancellationToken cancellationToken
    )
    {
        await _mediator.Send(new LoadActorBehaviorTreeShapes(), cancellationToken);
        await _mediator.Send(new LoadDefaultActorBehaviorTree(), cancellationToken);
    }
}
