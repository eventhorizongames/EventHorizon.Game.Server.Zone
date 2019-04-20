using System.Collections.Concurrent;
using System.Collections.Generic;
using EventHorizon.Zone.System.ClientEntities.Api;

namespace EventHorizon.Zone.System.ClientEntities.State
{
    public interface ClientEntityInstanceRepository
    {
        void Add(IClientEntityInstance clientEntityInstance);
        IEnumerable<IClientEntityInstance> All();
    }

    public class ClientEntityInstanceInMemoryRepository : ClientEntityInstanceRepository
    {
        private static readonly ConcurrentDictionary<string, IClientEntityInstance> INSTANCE_MAP = new ConcurrentDictionary<string, IClientEntityInstance>();
        public void Add(IClientEntityInstance entity)
        {
            INSTANCE_MAP.AddOrUpdate(entity.Id, entity, (key, old) => entity);
        }
        public IEnumerable<IClientEntityInstance> All()
        {
            return INSTANCE_MAP.Values;
        }
    }
}