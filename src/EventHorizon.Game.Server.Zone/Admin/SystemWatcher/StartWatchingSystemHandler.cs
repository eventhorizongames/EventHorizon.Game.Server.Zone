using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Admin.SystemWatcher.State;
using EventHorizon.Game.Server.Zone.External.Info;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace EventHorizon.Game.Server.Zone.Admin.SystemWatcher
{
    public struct StartWatchingSystemHandler : INotificationHandler<StartWatchingSystemEvent>
    {
        static FileSystemWatcher Watcher;

        readonly ISystemWatcherState _systemWatcherState;
        readonly ServerInfo _serverInfo;
        public StartWatchingSystemHandler(
            ISystemWatcherState systemWatcherState,
            ServerInfo serverInfo
        )
        {
            _systemWatcherState = systemWatcherState;
            _serverInfo = serverInfo;
        }

        public Task Handle(StartWatchingSystemEvent notification, CancellationToken cancellationToken)
        {
            if (Watcher != null)
            {
                Watcher.Dispose();
            }
            Watcher = new FileSystemWatcher();
            Watcher.IncludeSubdirectories = true;
            Watcher.NotifyFilter = NotifyFilters.LastAccess
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.FileName
                                 | NotifyFilters.DirectoryName;
            Watcher.Path = _serverInfo.AssetsPath;
            Watcher.Changed += OnChanged;
            Watcher.Created += OnChanged;
            Watcher.Deleted += OnChanged;

            // Begin watching.
            Watcher.EnableRaisingEvents = true;

            return Task.CompletedTask;
        }

        private void OnChanged(object source, FileSystemEventArgs e) =>
            _systemWatcherState.SetToPendingReload();
    }
}