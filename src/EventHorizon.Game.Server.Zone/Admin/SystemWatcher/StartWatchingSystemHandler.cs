using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Admin.SystemWatcher.State;
using EventHorizon.Zone.Core.Model.Info;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace EventHorizon.Game.Server.Zone.Admin.SystemWatcher
{
    public struct StartWatchingSystemHandler : INotificationHandler<StartWatchingSystemEvent>
    {
        static FileSystemWatcher ADMIN_WATCHER;
        static FileSystemWatcher CLIENT_WATCHER;
        static FileSystemWatcher I18N_WATCHER;
        static FileSystemWatcher SERVER_WATCHER;

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
            ADMIN_WATCHER?.Dispose();
            CLIENT_WATCHER?.Dispose();
            I18N_WATCHER?.Dispose();
            SERVER_WATCHER?.Dispose();

            ADMIN_WATCHER = CreateWatcherFor(
                _serverInfo.AdminPath
            );
            CLIENT_WATCHER = CreateWatcherFor(
                _serverInfo.ClientPath
            );
            I18N_WATCHER = CreateWatcherFor(
                _serverInfo.I18nPath
            );
            SERVER_WATCHER = CreateWatcherFor(
                _serverInfo.ServerPath
            );

            return Task.CompletedTask;
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

        private void OnChanged(object source, FileSystemEventArgs e) =>
            _systemWatcherState.SetToPendingReload();
    }
}