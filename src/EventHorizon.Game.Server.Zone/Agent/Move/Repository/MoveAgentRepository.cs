using System.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace EventHorizon.Game.Server.Zone.Agent.Move.Repository.Impl
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