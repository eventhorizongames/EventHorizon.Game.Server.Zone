namespace EventHorizon.Zone.System.ClientEntities.State
{
    using EventHorizon.Zone.System.ClientEntities.Model;
    using global::System.Collections.Concurrent;
    using global::System.Collections.Generic;

    public class ClientEntityInMemoryRepository : ClientEntityRepository
    {
        private readonly ConcurrentDictionary<string, ClientEntity> INSTANCE_MAP = new ConcurrentDictionary<string, ClientEntity>();

        public ClientEntity Find(
            string id
        )
        {
            if (INSTANCE_MAP.TryGetValue(
                id,
                out var entity
            ))
            {
                return entity;
            }
            return default(ClientEntity);
        }

        public void Add(
            ClientEntity entity
        )
        {
            INSTANCE_MAP.AddOrUpdate(
                entity.ClientEntityId,
                entity,
                (_, __) => entity
            );
        }

        public IEnumerable<ClientEntity> All()
        {
            return INSTANCE_MAP.Values;
        }

        public void Remove(
            string id
        )
        {
            INSTANCE_MAP.TryRemove(
                id,
                out _
            );
        }
    }
}