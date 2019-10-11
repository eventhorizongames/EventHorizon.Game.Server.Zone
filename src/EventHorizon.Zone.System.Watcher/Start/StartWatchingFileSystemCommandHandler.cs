using System.IO;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Watcher.Events.Start;
using EventHorizon.Zone.System.Watcher.Model;
using EventHorizon.Zone.System.Watcher.State;
using MediatR;

namespace EventHorizon.Zone.System.Watcher.Start
{
    public struct StartWatchingFileSystemCommandHandler : IRequestHandler<StartWatchingFileSystemCommand>
    {
        readonly PendingReloadState _pendingReload;
        readonly FileSystemWatcherState _watcherState;

        public StartWatchingFileSystemCommandHandler(
            PendingReloadState pendingReload,
            FileSystemWatcherState watcherState
        )
        {
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
            watcher.NotifyFilter = NotifyFilters.LastAccess
                                 | NotifyFilters.LastWrite
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
            FileSystemEventArgs _
        ) => _pendingReload.SetToPending();
    }
}