namespace EventHorizon.Zone.System.Watcher.Model
{
    using global::System;
    using global::System.IO;

    public struct StandardPathWatcher : PathWatcher
    {
        public string Path { get; }
        FileSystemWatcher _fileWatcher;

        public StandardPathWatcher(
            string path,
            FileSystemWatcher fileWatcher
        )
        {
            Path = path;
            _fileWatcher = fileWatcher;
        }

        public void Dispose()
        {
            _fileWatcher?.Dispose();
        }
    }
}
