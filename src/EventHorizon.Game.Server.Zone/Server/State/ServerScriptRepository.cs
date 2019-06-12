using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using EventHorizon.Game.Server.Zone.Server.Model;

namespace EventHorizon.Game.Server.Zone.Admin.Server.State
{
    public interface ServerScriptRepository
    {
        void Clear();
        void Add(ServerScript serverScript);
        ServerScript Find(string name);
    }

    public class ServerScriptInMemoryRepository : ServerScriptRepository
    {
        private static readonly ConcurrentDictionary<string, ServerScript> MAP = new ConcurrentDictionary<string, ServerScript>();
        public void Add(ServerScript script)
        {
            MAP.AddOrUpdate(script.Id, script, (key, old) => script);
        }
        public void Clear()
        {
            MAP.Clear();
        }
        public ServerScript Find(
            string id
        )
        {
            return MAP.Values.FirstOrDefault(a => a.Id == id);
        }
    }
}