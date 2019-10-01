using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Api;
using MediatR;

namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Register
{
    public struct UnRegisterActorWithBehaviorTreeUpdateHandler : IRequestHandler<UnRegisterActorWithBehaviorTreeUpdate>
    {
        readonly ActorBehaviorTreeRepository _repository;
        public UnRegisterActorWithBehaviorTreeUpdateHandler(
            ActorBehaviorTreeRepository repository
        )
        {
            _repository = repository;
        }
        public Task<Unit> Handle(
            UnRegisterActorWithBehaviorTreeUpdate request,
            CancellationToken cancellationToken
        )
        {
            _repository.UnRegisterActorFromTree(
                request.ActorId,
                request.TreeId
            );
            return Unit.Task;
        }
    }
}