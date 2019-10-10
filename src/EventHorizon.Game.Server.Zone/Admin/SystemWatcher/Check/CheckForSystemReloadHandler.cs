using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Admin.SystemWatcher.State;
using EventHorizon.Zone.System.Admin.Plugin.Command.Events;
using EventHorizon.Zone.System.Admin.Plugin.Command.Model.Builder;
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
                    new AdminCommandEvent(
                        BuildAdminCommand.FromString(
                            "reload-system"
                        ),
                        null
                    )
                );
                _systemWatcherState.RemovePendingReload();
            }
        }
    }
}