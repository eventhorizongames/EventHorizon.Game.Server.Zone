using System.IO;
using System.Threading;
using System.Threading.Tasks;

using EventHorizon.Zone.System.Watcher.Events.Start;
using EventHorizon.Zone.System.Watcher.Model;
using EventHorizon.Zone.System.Watcher.State;

using MediatR;

using Microsoft.Extensions.Logging;

namespace EventHorizon.Zone.System.Watcher.Start
{
    public class StartWatchingFileSystemCommandHandler : IRequestHandler<StartWatchingFileSystemCommand>
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

        public Task<Unit> Handle(
            StartWatchingFileSystemCommand request,
            CancellationToken cancellationToken
        )
        {
            var path = request.Path;

            _watcherState.Get(
                path
            )?.Dispose();
            _watcherState.Remove(
                path
            );

            _watcherState.Add(
                path,
                new StandardPathWatcher(
                    path,
                    CreateWatcherFor(
                        path
                    )
                )
            );

            return Unit.Task;
        }

        public FileSystemWatcher CreateWatcherFor(
            string path
        )
        {
            var watcher = new FileSystemWatcher();
            watcher.IncludeSubdirectories = true;
            watcher.NotifyFilter = NotifyFilters.LastWrite
                                 | NotifyFilters.FileName
                                 | NotifyFilters.DirectoryName;
            watcher.Path = path;
            watcher.Changed += OnChanged;
            watcher.Created += OnChanged;
            watcher.Deleted += OnChanged;

            // Begin watching.
            watcher.EnableRaisingEvents = true;

            return watcher;
        }

        private void OnChanged(
            object source,
            FileSystemEventArgs args
        )
        {
            _logger.LogDebug(
                "{ChangeType} triggered by {FullPath}",
                args.ChangeType.ToString(),
                args.FullPath
            );
            _pendingReload.SetToPending();
        }
    }
}
