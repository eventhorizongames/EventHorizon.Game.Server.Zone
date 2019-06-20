using System.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace EventHorizon.Game.Server.Zone.Agent.Move.Repository.Impl
{
    public class MoveAgentRepository : IMoveAgentRepository
    {
        private static readonly ConcurrentDictionary<long, long> ENTITIES = new ConcurrentDictionary<long, long>();
        public void Add(long entityId)
        {
            ENTITIES.TryAdd(entityId, entityId);
        }

        public IEnumerable<long> All()
        {
            return ENTITIES.Values;
        }

        public void Remove(long entityId)
        {
            ENTITIES.TryRemove(entityId, out _);
        }

        public bool Contains(
            long entityId
        )
        {
            return ENTITIES.ContainsKey(
                entityId
            );
        }
    }
}