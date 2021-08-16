namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.State.Queue
{
    using global::System.Collections.Concurrent;

    public class InMemoryActorBehaviorTickQueue : ActorBehaviorTickQueue
    {
        private readonly ConcurrentQueue<ActorBehaviorTick> ACTOR_BEHAVIOR_TICKS = new ConcurrentQueue<ActorBehaviorTick>();
        private readonly ConcurrentQueue<ActorBehaviorTick> TO_REGISTER = new ConcurrentQueue<ActorBehaviorTick>();

        public bool Dequeue(
            out ActorBehaviorTick actorBehaviorTick
        )
        {
            return ACTOR_BEHAVIOR_TICKS.TryDequeue(
                out actorBehaviorTick
            );
        }

        public void PrimeQueueWithRegisteredTicks()
        {
            while (TO_REGISTER.TryDequeue(
                out var actorBehaviorTick
            ))
            {
                ACTOR_BEHAVIOR_TICKS.Enqueue(
                    actorBehaviorTick
                );
            }
        }

        public void Register(
            string shapeId,
            long actorId
        )
        {
            TO_REGISTER.Enqueue(
                new ActorBehaviorTick(
                    shapeId,
                    actorId
                )
            );
        }

        public void RegisterFailed(
            ActorBehaviorTick actorBehaviorTick
        )
        {
            // TODO: This will check the failed count.
            // If over a threshold, ignore?
            TO_REGISTER.Enqueue(
                new ActorBehaviorTick(
                    actorBehaviorTick.FailedCount + 1,
                    actorBehaviorTick.ShapeId,
                    actorBehaviorTick.ActorId
                )
            );
        }
    }
}
