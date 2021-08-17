namespace EventHorizon.Zone.System.Watcher.State
{
    using EventHorizon.Zone.System.Watcher.Model;

    using global::System.Collections.Concurrent;

    public class InMemoryFileSystemWatcherState
        : FileSystemWatcherState
    {
        private readonly ConcurrentDictionary<string, PathWatcher> _map = new();

        public void Add(
            string path,
            PathWatcher watcher
        )
        {
            _map.AddOrUpdate(
                path,
                watcher,
                (_, _) => watcher
            );
        }

        public PathWatcher? Get(
            string path
        )
        {
            if (_map.TryGetValue(
                path,
                out var watcher
            ))
            {
                return watcher;
            }
            return default;
        }

        public void Remove(string path)
        {
            _map.TryRemove(
                path,
                out _
            );
        }
    }
}
