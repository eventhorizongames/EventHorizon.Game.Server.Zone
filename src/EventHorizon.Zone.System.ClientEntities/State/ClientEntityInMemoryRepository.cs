namespace EventHorizon.Zone.System.ClientEntities.State;

using EventHorizon.Zone.System.ClientEntities.Model;

using global::System.Collections.Concurrent;
using global::System.Collections.Generic;

public class ClientEntityInMemoryRepository : ClientEntityRepository
{
    private readonly ConcurrentDictionary<string, ClientEntity> _map = new();

    public ClientEntity Find(
        string id
    )
    {
        if (_map.TryGetValue(
            id,
            out var entity
        ))
        {
            return entity;
        }
        return default;
    }

    public void Add(
        ClientEntity entity
    )
    {
        _map.AddOrUpdate(
            entity.ClientEntityId,
            entity,
            (_, _) => entity
        );
    }

    public IEnumerable<ClientEntity> All()
    {
        return _map.Values;
    }

    public void Remove(
        string id
    )
    {
        _map.TryRemove(
            id,
            out _
        );
    }
}
