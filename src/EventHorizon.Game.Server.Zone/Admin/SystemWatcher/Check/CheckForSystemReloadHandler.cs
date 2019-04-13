using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Admin.SystemWatcher.State;
using EventHorizon.Game.Server.Zone.Events.Admin;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventHorizon.Game.Server.Zone.Admin.SystemWatcher.Check
{
    public struct CheckForSystemReloadHandler : INotificationHandler<CheckForSystemReloadEvent>
    {
        readonly IMediator _mediator;
        readonly ILogger _logger;
        readonly ISystemWatcherState _systemWatcherState;

        public CheckForSystemReloadHandler(
            IMediator mediator,
            ILogger<CheckForSystemReloadHandler> logger,
            ISystemWatcherState systemWatcherState
        )
        {
            _mediator = mediator;
            _logger = logger;
            _systemWatcherState = systemWatcherState;
        }

        public async Task Handle(CheckForSystemReloadEvent notification, CancellationToken cancellationToken)
        {
            if (_systemWatcherState.PendingReload)
            {
                _logger.LogInformation("Running System Reload");
                await _mediator.Publish(
                    new AdminCommandReloadSystemEvent()
                );
                _systemWatcherState.RemovePendingReload();
            }
        }
    }
}