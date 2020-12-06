namespace EventHorizon.Zone.System.Server.Scripts.State
{
    using global::System.Collections.Concurrent;
    using EventHorizon.Zone.System.Server.Scripts.Exceptions;
    using EventHorizon.Zone.System.Server.Scripts.Model;

    public class ServerScriptInMemoryRepository : ServerScriptRepository
    {
        private readonly ConcurrentDictionary<string, ServerScript> MAP = new ConcurrentDictionary<string, ServerScript>();

        public void Add(
            ServerScript script
        )
        {
            MAP.TryRemove(script.Id, out _);
            MAP.TryAdd(
                script.Id,
                script
            );
        }

        public ServerScript Find(
            string id
        )
        {
            if (MAP.TryGetValue(
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