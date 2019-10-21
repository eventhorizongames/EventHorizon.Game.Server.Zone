using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using EventHorizon.Zone.System.Server.Scripts.Model.Details;

namespace EventHorizon.Zone.System.Server.Scripts.State
{
    public class ServerScriptDetailsInMemoryRepository : ServerScriptDetailsRepository
    {
        private readonly ConcurrentDictionary<string, ServerScriptDetails> MAP = new ConcurrentDictionary<string, ServerScriptDetails>();

        public void Add(
            string id,
            ServerScriptDetails script
        )
        {
            MAP.AddOrUpdate(
                id,
                script,
                (key, old) => script
            );
        }

        public IEnumerable<ServerScriptDetails> Where(
            Func<ServerScriptDetails, bool> query
        )
        {
            return MAP.Values.Where(
                query
            );
        }
    }
}