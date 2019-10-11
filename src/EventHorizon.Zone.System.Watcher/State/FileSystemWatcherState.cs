using EventHorizon.Zone.System.Watcher.Model;

namespace EventHorizon.Zone.System.Watcher.State
{
    public interface FileSystemWatcherState
    {
        void Add(
            string path,
            PathWatcher watcher
        );
        PathWatcher Get(
            string path
        );
        void Remove(
            string path
        );
    }
}