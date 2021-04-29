namespace EventHorizon.Zone.System.Server.Scripts.State
{
    using EventHorizon.Zone.System.Server.Scripts.Exceptions;
    using EventHorizon.Zone.System.Server.Scripts.Model.Details;
    using global::System;
    using global::System.Collections.Concurrent;
    using global::System.Collections.Generic;
    using global::System.Linq;

    public class ServerScriptDetailsInMemoryRepository
        : ServerScriptDetailsRepository
    {
        private readonly ConcurrentDictionary<string, ServerScriptDetails> _map = new();

        public IEnumerable<ServerScriptDetails> All
            => _map.Values;

        public void Add(
            string id,
            ServerScriptDetails script
        )
        {
            _map.AddOrUpdate(
                id,
                script,
                (key, old) => script
            );
        }

        public void Clear()
        {
            _map.Clear();
        }

        public ServerScriptDetails Find(
            string id
        )
        {
            if (_map.TryGetValue(
                id,
                out var value
            ))
            {
                return value;
            }

            throw new ServerScriptDetailsNotFound(
                id,
                "Failed to find Server Script Details"
            );
        }

        public IEnumerable<ServerScriptDetails> Where(
            Func<ServerScriptDetails, bool> query
        )
        {
            return _map.Values.Where(
                query
            );
        }
    }
}
