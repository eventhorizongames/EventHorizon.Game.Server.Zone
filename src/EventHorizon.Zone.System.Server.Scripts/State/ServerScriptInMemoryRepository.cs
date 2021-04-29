namespace EventHorizon.Zone.System.Server.Scripts.State
{
    using EventHorizon.Zone.System.Server.Scripts.Api;
    using EventHorizon.Zone.System.Server.Scripts.Exceptions;
    using EventHorizon.Zone.System.Server.Scripts.Model;
    using global::System.Collections.Concurrent;
    using global::System.Collections.Generic;

    public class ServerScriptInMemoryRepository
        : ServerScriptRepository
    {
        private readonly ConcurrentDictionary<string, ServerScript> _map = new();

        public IEnumerable<ServerScript> All
            => _map.Values;

        public void Clear()
        {
            _map.Clear();
        }

        public void Add(
            ServerScript script
        )
        {
            _map.AddOrUpdate(
                script.Id,
                script,
                (_, __) => script
            );
        }

        public ServerScript Find(
            string id
        )
        {
            if (_map.TryGetValue(
                id,
                out var script
            ))
            {
                return script;
            }

            throw new ServerScriptNotFound(
                id,
                "ServerScriptInMemoryRepository did not find the script."
            );
        }
    }
}