using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Agent.Behavior.Api;
using MediatR;

namespace EventHorizon.Zone.System.Agent.Behavior.Register
{
    public struct RegisterActorWithBehaviorTreeUpdate : IRequest
    {
        public long ActorId { get; }
        public string TreeId { get; set; }

        public RegisterActorWithBehaviorTreeUpdate(
            long actorId,
            string treeId
        )
        {
            ActorId = actorId;
            TreeId = treeId;
        }

        public struct RegisterActorWithBehaviorTreeUpdateHandler : IRequestHandler<RegisterActorWithBehaviorTreeUpdate>
        {
            readonly ActorBehaviorTreeRepository _repository;
            public RegisterActorWithBehaviorTreeUpdateHandler(
                ActorBehaviorTreeRepository repository
            )
            {
                _repository = repository;
            }
            public Task<Unit> Handle(
                RegisterActorWithBehaviorTreeUpdate request,
                CancellationToken cancellationToken
            )
            {
                _repository.RegisterActorToTree(
                    request.ActorId,
                    request.TreeId
                );
                return Unit.Task;
            }
        }
    }
}