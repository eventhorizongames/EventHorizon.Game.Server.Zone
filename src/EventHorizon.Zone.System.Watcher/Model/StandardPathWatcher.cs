namespace EventHorizon.Zone.System.Watcher.Model
{
    using global::System.IO;

    public struct StandardPathWatcher : PathWatcher
    {
        private readonly FileSystemWatcher _fileWatcher;

        public string Path { get; }

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
