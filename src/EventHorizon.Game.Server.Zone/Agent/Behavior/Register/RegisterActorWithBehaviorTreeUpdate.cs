using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Agent.Behavior.Register
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
            public Task<Unit> Handle(
                RegisterActorWithBehaviorTreeUpdate request,
                CancellationToken cancellationToken
            )
            {
                return Unit.Task;
            }
        }
    }
}