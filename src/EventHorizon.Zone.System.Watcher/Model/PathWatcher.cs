using System;
using System.IO;

namespace EventHorizon.Zone.System.Watcher.Model
{
    public interface PathWatcher : IDisposable
    {
        string Path { get; }
    }
}