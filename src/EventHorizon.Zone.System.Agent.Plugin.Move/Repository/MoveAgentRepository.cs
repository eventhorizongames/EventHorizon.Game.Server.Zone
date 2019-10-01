using System.Collections.Concurrent;
using EventHorizon.Zone.System.Agent.Model.State;

namespace EventHorizon.Zone.System.Agent.Move.Repository
{
    public class MoveAgentRepository : IMoveAgentRepository
    {
        private static readonly ConcurrentQueue<long> ENTITIES = new ConcurrentQueue<long>();
        private static readonly ConcurrentQueue<long> TO_REGISTER = new ConcurrentQueue<long>();

        public void Register(long entityId)
        {
            TO_REGISTER.Enqueue(entityId);
        }

        public bool Dequeue(out long entityId)
        {
            return ENTITIES.TryDequeue(out entityId);
        }

        public void MergeRegisteredIntoQueue()
        {
            long entityId = 0;
            while (TO_REGISTER.TryDequeue(out entityId))
            {
                ENTITIES.Enqueue(
                    entityId
                );
            }
        }
    }
}