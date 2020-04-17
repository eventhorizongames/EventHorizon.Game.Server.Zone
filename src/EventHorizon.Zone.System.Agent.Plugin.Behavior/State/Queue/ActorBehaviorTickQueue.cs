namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.State.Queue
{
    public interface ActorBehaviorTickQueue
    {
        void Register(
            string shapeId,
            long actorId
        );
        void RegisterFailed(
            ActorBehaviorTick actorBehaviorTick
        );
        bool Dequeue(
            out ActorBehaviorTick actorBehaviorTick
        );
        void PrimeQueueWithRegisteredTicks();
    }

    public struct ActorBehaviorTick
    {
        public int FailedCount { get; }
        public string ShapeId { get; }
        public long ActorId { get; }

        public ActorBehaviorTick(
            string shapeId,
            long actorId
        ) : this(
            0,
            shapeId,
            actorId
        )
        { }

        public ActorBehaviorTick(
            int failedCount,
            string shapeId,
            long actorId
        )
        {
            FailedCount = failedCount;
            ShapeId = shapeId;
            ActorId = actorId;
        }
    }
}