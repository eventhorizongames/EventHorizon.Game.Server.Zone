using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Agent.Behavior.Api;
using MediatR;

namespace EventHorizon.Zone.System.Agent.Behavior.UnRegister
{
    public struct UnRegisterActorWithBehaviorTreeUpdate : IRequest
    {
        public long ActorId { get; }
        public string TreeId { get; set; }

        public UnRegisterActorWithBehaviorTreeUpdate(
            long actorId,
            string treeId
        )
        {
            ActorId = actorId;
            TreeId = treeId;
        }

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
}