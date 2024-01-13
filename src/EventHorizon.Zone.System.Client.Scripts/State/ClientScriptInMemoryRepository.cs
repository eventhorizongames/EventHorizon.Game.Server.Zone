namespace EventHorizon.Zone.System.Client.Scripts.State;

using EventHorizon.Zone.System.Client.Scripts.Api;
using EventHorizon.Zone.System.Client.Scripts.Model;

using global::System.Collections.Concurrent;
using global::System.Collections.Generic;

public class ClientScriptInMemoryRepository
    : ClientScriptRepository
{
    private readonly ConcurrentDictionary<string, ClientScript> _map = new();

    public void Add(
        ClientScript script
    )
    {
        _map.AddOrUpdate(
            script.Name,
            script,
            (_, _) => script
        );
    }

    public IEnumerable<ClientScript> All()
    {
        return _map.Values;
    }
}
