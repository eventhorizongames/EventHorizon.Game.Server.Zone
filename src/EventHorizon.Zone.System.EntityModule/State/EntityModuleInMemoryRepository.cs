namespace EventHorizon.Zone.System.EntityModule.State
{
    using EventHorizon.Zone.System.EntityModule.Api;
    using EventHorizon.Zone.System.EntityModule.Model;

    using global::System.Collections.Concurrent;
    using global::System.Collections.Generic;

    public class EntityModuleInMemoryRepository
        : EntityModuleRepository
    {
        // TODO: Remove static and test
        private static readonly ConcurrentDictionary<string, EntityScriptModule> BASE_MODULE_MAP = new();
        private static readonly ConcurrentDictionary<string, EntityScriptModule> PLAYER_MODULE_MAP = new();

        public void AddBaseModule(
            EntityScriptModule module
        )
        {
            BASE_MODULE_MAP.AddOrUpdate(
                module.Name,
                module,
                (_, _) => module
            );
        }

        public void AddPlayerModule(
            EntityScriptModule module
        )
        {
            PLAYER_MODULE_MAP.AddOrUpdate(
                module.Name,
                module,
                (_, _) => module
            );
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
