using System.Threading;
using System.Threading.Tasks;

using EventHorizon.Zone.System.Admin.Plugin.Command.Events;
using EventHorizon.Zone.System.Admin.Plugin.Command.Model.Builder;
using EventHorizon.Zone.System.Watcher.State;

using MediatR;

using Microsoft.Extensions.Logging;

namespace EventHorizon.Zone.System.Watcher.Check
{
    public class CheckPendingReloadEventHandler : INotificationHandler<CheckPendingReloadEvent>
    {
        readonly IMediator _mediator;
        readonly ILogger _logger;
        readonly PendingReloadState _pendingReload;

        public CheckPendingReloadEventHandler(
            IMediator mediator,
            ILogger<CheckPendingReloadEventHandler> logger,
            PendingReloadState pendingReload
        )
        {
            _mediator = mediator;
            _logger = logger;
            _pendingReload = pendingReload;
        }

        public async Task Handle(
            CheckPendingReloadEvent notification,
            CancellationToken cancellationToken
        )
        {
            if (_pendingReload.IsPending)
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
                _pendingReload.RemovePending();
            }
        }
    }
}
