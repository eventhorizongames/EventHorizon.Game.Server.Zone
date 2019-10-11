using System;
using System.IO;

namespace EventHorizon.Zone.System.Watcher.Model
{
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