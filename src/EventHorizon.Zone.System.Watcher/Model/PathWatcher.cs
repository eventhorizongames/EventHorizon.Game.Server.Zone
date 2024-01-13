namespace EventHorizon.Zone.System.Watcher.Model;

using global::System;
using global::System.IO;

public interface PathWatcher : IDisposable
{
    string Path { get; }
}
