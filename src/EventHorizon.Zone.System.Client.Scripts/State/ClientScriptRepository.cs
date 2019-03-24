using System.Collections.Concurrent;
using System.Collections.Generic;
using EventHorizon.Zone.System.Client.Scripts.Model;

namespace EventHorizon.Zone.System.Client.Scripts.State
{
    public interface ClientScriptRepository
    {
        void Add(ClientScript clientScript);
        IEnumerable<ClientScript> All();
    }

    public class ClientScriptInMemoryRepository : ClientScriptRepository
    {
        private static readonly ConcurrentDictionary<string, ClientScript> SCRIPT_MAP = new ConcurrentDictionary<string, ClientScript>();
        public void Add(ClientScript script)
        {
            SCRIPT_MAP.AddOrUpdate(script.Name, script, (key, old) => script);
        }
        public IEnumerable<ClientScript> All()
        {
            return SCRIPT_MAP.Values;
        }
    }
}