namespace EventHorizon.Zone.System.Server.Scripts.State
{
    using global::System.Collections.Concurrent;
    using EventHorizon.Zone.System.Server.Scripts.Model;
    using EventHorizon.Zone.System.Server.Scripts.Exceptions;
    using EventHorizon.Zone.System.Server.Scripts.Api;

    public class ServerScriptInMemoryRepository
        : ServerScriptRepository
    {
        private readonly ConcurrentDictionary<string, ServerScript> _map = new ConcurrentDictionary<string, ServerScript>();

        public void Clear()
        {
            _map.Clear();
        }

        public void Add(
            ServerScript script
        )
        {
            _map.TryRemove(script.Id, out _);
            _map.TryAdd(
                script.Id,
                script
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