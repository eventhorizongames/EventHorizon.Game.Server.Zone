using MediatR;

namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Register
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
    }
}