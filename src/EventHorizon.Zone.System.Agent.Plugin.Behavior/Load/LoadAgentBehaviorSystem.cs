using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Load
{
    public struct LoadAgentBehaviorSystem : IRequest
    {
        public struct LoadAgentBehaviorSystemHandler : IRequestHandler<LoadAgentBehaviorSystem>
        {
            readonly IMediator _mediator;

            public LoadAgentBehaviorSystemHandler(
                IMediator mediator
            )
            {
                _mediator = mediator;
            }

            public async Task<Unit> Handle(
                LoadAgentBehaviorSystem request,
                CancellationToken cancellationToken
            )
            {
                await _mediator.Send(
                    new LoadActorBehaviorScripts()
                );
                await _mediator.Send(
                    new LoadActorBehaviorTreeShapes()
                );
                await _mediator.Send(
                    new LoadDefaultActorBehaviorTree()
                );

                return Unit.Value;
            }
        }
    }
}