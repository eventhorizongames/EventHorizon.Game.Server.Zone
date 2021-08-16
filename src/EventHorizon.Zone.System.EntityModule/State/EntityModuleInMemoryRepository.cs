using System.Collections.Concurrent;
using System.Collections.Generic;

using EventHorizon.Zone.System.EntityModule.Api;
using EventHorizon.Zone.System.EntityModule.Model;

namespace EventHorizon.Zone.System.EntityModule.State
{
    public class EntityModuleInMemoryRepository : EntityModuleRepository
    {
        private static readonly ConcurrentDictionary<string, EntityScriptModule> BASE_MODULE_MAP = new ConcurrentDictionary<string, EntityScriptModule>();
        private static readonly ConcurrentDictionary<string, EntityScriptModule> PLAYER_MODULE_MAP = new ConcurrentDictionary<string, EntityScriptModule>();

        public void AddBaseModule(EntityScriptModule module)
        {
            BASE_MODULE_MAP.AddOrUpdate(module.Name, module, (key, old) => module);
        }
        public void AddPlayerModule(EntityScriptModule module)
        {
            PLAYER_MODULE_MAP.AddOrUpdate(module.Name, module, (key, old) => module);
        }
        public IEnumerable<EntityScriptModule> ListOfAllBaseModules()
        {
            return BASE_MODULE_MAP.Values;
        }
        public IEnumerable<EntityScriptModule> ListOfAllPlayerModules()
        {
            return PLAYER_MODULE_MAP.Values;
        }
    }
}
