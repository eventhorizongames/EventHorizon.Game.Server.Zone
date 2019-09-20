using System.Collections.Concurrent;
using System.Linq;
using EventHorizon.Zone.System.Server.Scripts.Model;

namespace EventHorizon.Game.Server.Zone.Admin.Server.State
{
    public class ServerScriptInMemoryRepository : ServerScriptRepository
    {
        private static readonly ConcurrentDictionary<string, ServerScript> MAP = new ConcurrentDictionary<string, ServerScript>();
        public void Add(
            ServerScript script
        )
        {
            MAP.AddOrUpdate(
                script.Id, 
                script, 
                (key, old) => script
            );
        }
        public void Clear()
        {
            MAP.Clear();
        }
        public ServerScript Find(
            string id
        )
        {
            return MAP.Values.FirstOrDefault(
                a => a.Id == id
            );
        }
    }
}