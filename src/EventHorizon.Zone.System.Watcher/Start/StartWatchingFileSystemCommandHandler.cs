namespace EventHorizon.Zone.System.Watcher.Start;

using EventHorizon.Zone.System.Watcher.Events.Start;
using EventHorizon.Zone.System.Watcher.Model;
using EventHorizon.Zone.System.Watcher.State;
using global::System.IO;
using global::System.Threading;
using global::System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

public class StartWatchingFileSystemCommandHandler
    : IRequestHandler<StartWatchingFileSystemCommand>
{
    private readonly ILogger<StartWatchingFileSystemCommandHandler> _logger;
    private readonly PendingReloadState _pendingReload;
    private readonly FileSystemWatcherState _watcherState;

    public StartWatchingFileSystemCommandHandler(
        ILogger<StartWatchingFileSystemCommandHandler> logger,
        PendingReloadState pendingReload,
        FileSystemWatcherState watcherState
    )
    {
        _logger = logger;
        _pendingReload = pendingReload;
        _watcherState = watcherState;
    }

    public Task Handle(
        StartWatchingFileSystemCommand request,
        CancellationToken cancellationToken
    )
    {
        var path = request.Path;

        _watcherState.Get(path)?.Dispose();
        _watcherState.Remove(path);

        _watcherState.Add(path, new StandardPathWatcher(path, CreateWatcherFor(path)));

        return Task.CompletedTask;
    }

    public FileSystemWatcher CreateWatcherFor(string path)
    {
        var watcher = new FileSystemWatcher
        {
            IncludeSubdirectories = true,
            NotifyFilter =
                NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName,
            Path = path
        };
        watcher.Changed += OnChanged;
        watcher.Created += OnChanged;
        watcher.Deleted += OnChanged;

        // Begin watching.
        watcher.EnableRaisingEvents = true;

        return watcher;
    }

    private void OnChanged(object source, FileSystemEventArgs args)
    {
        _logger.LogDebug(
            "{ChangeType} triggered by {FullPath}",
            args.ChangeType.ToString(),
            args.FullPath
        );
        _pendingReload.SetToPending();
    }
}
