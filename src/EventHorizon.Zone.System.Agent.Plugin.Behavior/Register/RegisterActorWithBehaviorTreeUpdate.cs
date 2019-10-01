using MediatR;

namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Register
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
    }
}