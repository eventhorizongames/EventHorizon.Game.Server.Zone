namespace EventHorizon.Zone.System.ClientEntities.State
{
    using global::System.Collections.Concurrent;
    using global::System.Collections.Generic;
    using EventHorizon.Zone.System.ClientEntities.Model;

    public class ClientEntityInMemoryRepository : ClientEntityRepository
    {
        private readonly ConcurrentDictionary<string, ClientEntity> INSTANCE_MAP = new ConcurrentDictionary<string, ClientEntity>();

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
    }
}