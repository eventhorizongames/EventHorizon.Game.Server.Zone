using System.Collections.Concurrent;
using System.Linq;
using EventHorizon.Zone.System.Server.Scripts.Exceptions;
using EventHorizon.Zone.System.Server.Scripts.Model;

namespace EventHorizon.Zone.System.Server.Scripts.State
{
    public class ServerScriptInMemoryRepository : ServerScriptRepository
    {
        private readonly ConcurrentDictionary<string, ServerScript> MAP = new ConcurrentDictionary<string, ServerScript>();

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

        public ServerScript Find(
            string id
        )
        {
            var script = MAP.Values.FirstOrDefault(
                a => a.Id == id
            );
            if (script == null)
            {
                throw new ServerScriptNotFound(
                    id,
                    "ServerScriptInMemoryRepository did not find the script."
                );
            }
            return script;
        }
    }
}