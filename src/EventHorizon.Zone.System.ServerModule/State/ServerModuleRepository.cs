using System.Collections.Concurrent;
using System.Collections.Generic;
using EventHorizon.Zone.System.ServerModule.Model;

namespace EventHorizon.Zone.System.ServerModule.State
{
    public interface ServerModuleRepository
    {
        void Add(ServerModuleScripts serverModule);
        IEnumerable<ServerModuleScripts> All();
    }

    public class ServerModuleInMemoryRepository : ServerModuleRepository
    {
        private static readonly ConcurrentDictionary<string, ServerModuleScripts> SCRIPT_MAP = new ConcurrentDictionary<string, ServerModuleScripts>();
        public void Add(ServerModuleScripts script)
        {
            SCRIPT_MAP.AddOrUpdate(script.Name, script, (key, old) => script);
        }
        public IEnumerable<ServerModuleScripts> All()
        {
            return SCRIPT_MAP.Values;
        }
    }
}