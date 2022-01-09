namespace EventHorizon.Zone.System.EntityModule.State;

using EventHorizon.Zone.System.EntityModule.Api;
using EventHorizon.Zone.System.EntityModule.Model;

using global::System.Collections.Concurrent;
using global::System.Collections.Generic;

public class EntityModuleInMemoryRepository
    : EntityModuleRepository
{
    private readonly ConcurrentDictionary<string, EntityScriptModule> _baseModuleMap = new();
    private readonly ConcurrentDictionary<string, EntityScriptModule> _playerModuleMap = new();

    public void Clear()
    {
        _baseModuleMap.Clear();
        _playerModuleMap.Clear();
    }

    public void AddBaseModule(
        EntityScriptModule module
    )
    {
        _baseModuleMap.AddOrUpdate(
            module.Name,
            module,
            (_, _) => module
        );
    }

    public void AddPlayerModule(
        EntityScriptModule module
    )
    {
        _playerModuleMap.AddOrUpdate(
            module.Name,
            module,
            (_, _) => module
        );
    }

    public IEnumerable<EntityScriptModule> ListOfAllBaseModules()
    {
        return _baseModuleMap.Values;
    }

    public IEnumerable<EntityScriptModule> ListOfAllPlayerModules()
    {
        return _playerModuleMap.Values;
    }
}
