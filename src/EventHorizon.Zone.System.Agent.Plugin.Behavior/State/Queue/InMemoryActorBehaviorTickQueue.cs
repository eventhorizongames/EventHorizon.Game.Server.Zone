namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.State.Queue
{
    using global::System.Collections.Concurrent;

    public class InMemoryActorBehaviorTickQueue
        : ActorBehaviorTickQueue
    {
        private readonly ConcurrentQueue<ActorBehaviorTick> _actionBehaviorTicks = new();
        private readonly ConcurrentQueue<ActorBehaviorTick> _toRegister = new();

        public bool Dequeue(
            out ActorBehaviorTick actorBehaviorTick
        )
        {
            return _actionBehaviorTicks.TryDequeue(
                out actorBehaviorTick
            );
        }

        public void PrimeQueueWithRegisteredTicks()
        {
            while (_toRegister.TryDequeue(
                out var actorBehaviorTick
            ))
            {
                _actionBehaviorTicks.Enqueue(
                    actorBehaviorTick
                );
            }
        }

        public void Register(
            string shapeId,
            long actorId
        )
        {
            _toRegister.Enqueue(
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
            _toRegister.Enqueue(
                new ActorBehaviorTick(
                    actorBehaviorTick.FailedCount + 1,
                    actorBehaviorTick.ShapeId,
                    actorBehaviorTick.ActorId
                )
            );
        }
    }
}
