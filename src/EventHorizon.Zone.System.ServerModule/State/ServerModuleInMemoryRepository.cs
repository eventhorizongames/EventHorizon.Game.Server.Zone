namespace EventHorizon.Zone.System.ServerModule.State
{
    using EventHorizon.Zone.System.ServerModule.Model;

    using global::System.Collections.Concurrent;
    using global::System.Collections.Generic;

    public class ServerModuleInMemoryRepository : ServerModuleRepository
    {
        private readonly ConcurrentDictionary<string, ServerModuleScripts> SCRIPT_MAP = new ConcurrentDictionary<string, ServerModuleScripts>();

        public void Add(
            ServerModuleScripts script
        )
        {
            SCRIPT_MAP.AddOrUpdate(
                script.Name,
                script,
                (key, old) => script
            );
        }

        public IEnumerable<ServerModuleScripts> All()
        {
            return SCRIPT_MAP.Values;
        }
    }
}
