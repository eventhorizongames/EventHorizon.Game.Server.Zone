namespace EventHorizon.Game.Server.Zone.Admin.FileSystem
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;

    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.System.Watcher.Events.Start;

    using MediatR;

    public class StartAdminFileSystemWatchingCommandHandler : IRequestHandler<StartAdminFileSystemWatchingCommand>
    {
        readonly IMediator _mediator;
        readonly ServerInfo _serverInfo;

        public StartAdminFileSystemWatchingCommandHandler(
            IMediator mediator,
            ServerInfo serverInfo
        )
        {
            _mediator = mediator;
            _serverInfo = serverInfo;
        }

        public async Task<Unit> Handle(
            StartAdminFileSystemWatchingCommand request,
            CancellationToken cancellationToken
        )
        {
            // Add File System Watcher for Server Path
            await _mediator.Send(
                new StartWatchingFileSystemCommand(
                    _serverInfo.ServerPath
                ),
                cancellationToken
            );

            // Add File System Watcher for Admin Path
            await _mediator.Send(
                new StartWatchingFileSystemCommand(
                    _serverInfo.AdminPath
                ),
                cancellationToken
            );

            // Add File System Watcher for I18n Path
            await _mediator.Send(
                new StartWatchingFileSystemCommand(
                    _serverInfo.I18nPath
                ),
                cancellationToken
            );

            // Add File System Watcher for Client Path
            #region Client Paths
            var clientPaths = new List<string>
            {
                "Assets",
                "Entity",
                "Gui",
                "Modules",
                "Particle",
                "Scripts",
                "ServerModule",
                "Skills",
            };

            foreach (var clientPath in clientPaths)
            {
                await _mediator.Send(
                    new StartWatchingFileSystemCommand(
                        Path.Combine(
                            _serverInfo.ClientPath,
                            clientPath
                        )
                    ),
                    cancellationToken
                );
            }

            #endregion

            // Add File System Watcher for Agent Reload Path
            await _mediator.Send(
                new StartWatchingFileSystemCommand(
                    Path.Combine(
                        _serverInfo.AppDataPath,
                        "Agent",
                        "Reload"
                    )
                ),
                cancellationToken
            );

            // Add File System Watcher for Player Path
            await _mediator.Send(
                new StartWatchingFileSystemCommand(
                    Path.Combine(
                        _serverInfo.AppDataPath,
                        "Player"
                    )
                ),
                cancellationToken
            );

            // Add File System Watcher for Player Path
            await _mediator.Send(
                new StartWatchingFileSystemCommand(
                    Path.Combine(
                        _serverInfo.AppDataPath,
                        "Entity"
                    )
                ),
                cancellationToken
            );

            await _mediator.Send(
                new StartWatchingFileSystemCommand(
                    _serverInfo.CoreMapPath
                ),
                cancellationToken
            );

            await _mediator.Send(
                new StartWatchingFileSystemCommand(
                    _serverInfo.SystemsPath
                ),
                cancellationToken
            );

            return Unit.Value;
        }
    }
}
