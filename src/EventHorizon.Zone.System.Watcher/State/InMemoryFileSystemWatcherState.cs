using System.Collections.Concurrent;
using EventHorizon.Zone.System.Watcher.Model;

namespace EventHorizon.Zone.System.Watcher.State
{
    public class InMemoryFileSystemWatcherState : FileSystemWatcherState
    {
        private readonly ConcurrentDictionary<string, PathWatcher> MAP = new ConcurrentDictionary<string, PathWatcher>();

        public void Add(
            string path,
            PathWatcher watcher
        )
        {
            MAP.AddOrUpdate(
                path,
                watcher,
                (_, __) => watcher
            );
        }

        public PathWatcher Get(
            string path
        )
        {
            if (MAP.TryGetValue(
                path,
                out var watcher
            ))
            {
                return watcher;
            }
            return default(PathWatcher);
        }

        public void Remove(string path)
        {
            MAP.TryRemove(
                path,
                out _
            );
        }
    }
}