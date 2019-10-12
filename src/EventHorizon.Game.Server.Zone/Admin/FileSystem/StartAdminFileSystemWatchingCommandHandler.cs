using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.System.Watcher.Events.Start;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Admin.FileSystem
{
    public struct StartAdminFileSystemWatchingCommandHandler : IRequestHandler<StartAdminFileSystemWatchingCommand>
    {
        readonly ServerInfo _serverInfo;
        readonly IMediator _mediator;

        public StartAdminFileSystemWatchingCommandHandler(
            ServerInfo serverInfo,
            IMediator mediator
        )
        {
            _serverInfo = serverInfo;
            _mediator = mediator;
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
                    )
            );
            
            // Add File System Watcher for Admin Path
            await _mediator.Send(
                new StartWatchingFileSystemCommand(
                        _serverInfo.AdminPath
                    )
            );

            // Add File System Watcher for I18n Path
            await _mediator.Send(
                new StartWatchingFileSystemCommand(
                        _serverInfo.I18nPath
                    )
            );
            
            // Add File System Watcher for Client Path
            await _mediator.Send(
                new StartWatchingFileSystemCommand(
                        _serverInfo.ClientPath
                    )
            );

            return Unit.Value;
        }
    }
}