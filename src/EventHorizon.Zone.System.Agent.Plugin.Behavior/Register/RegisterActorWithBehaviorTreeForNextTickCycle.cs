namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Register
{
    using MediatR;

    public struct RegisterActorWithBehaviorTreeForNextTickCycle : IRequest
    {
        public string ShapeId { get; }
        public long ActorId { get; }

        public RegisterActorWithBehaviorTreeForNextTickCycle(
            string shapeId,
            long actorId
        )
        {
            ShapeId = shapeId;
            ActorId = actorId;
        }
    }
}
