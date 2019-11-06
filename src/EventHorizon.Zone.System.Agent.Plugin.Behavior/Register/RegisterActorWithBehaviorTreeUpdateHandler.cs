using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Api;
using MediatR;

namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Register
{
    public class RegisterActorWithBehaviorTreeUpdateHandler : IRequestHandler<RegisterActorWithBehaviorTreeUpdate>
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