namespace EventHorizon.Zone.System.Watcher.State;

using EventHorizon.Zone.System.Watcher.Model;

public interface FileSystemWatcherState
{
    void Add(
        string path,
        PathWatcher watcher
    );

    PathWatcher? Get(
        string path
    );

    void Remove(
        string path
    );
}
