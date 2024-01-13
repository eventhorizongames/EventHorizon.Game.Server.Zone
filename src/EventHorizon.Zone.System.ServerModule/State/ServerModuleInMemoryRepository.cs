namespace EventHorizon.Zone.System.ServerModule.State;

using EventHorizon.Zone.System.ServerModule.Model;

using global::System.Collections.Concurrent;
using global::System.Collections.Generic;

public class ServerModuleInMemoryRepository
    : ServerModuleRepository
{
    private readonly ConcurrentDictionary<string, ServerModuleScripts> _map = new();

    public void Add(
        ServerModuleScripts script
    )
    {
        _map.AddOrUpdate(
            script.Name,
            script,
            (key, old) => script
        );
    }

    public IEnumerable<ServerModuleScripts> All()
    {
        return _map.Values;
    }
}
