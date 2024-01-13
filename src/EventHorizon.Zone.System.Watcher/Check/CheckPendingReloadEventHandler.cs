namespace EventHorizon.Zone.System.Watcher.Check;

using EventHorizon.Zone.System.Admin.Plugin.Command.Events;
using EventHorizon.Zone.System.Admin.Plugin.Command.Model.Builder;
using EventHorizon.Zone.System.Watcher.State;

using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

using Microsoft.Extensions.Logging;

public class CheckPendingReloadEventHandler
    : INotificationHandler<CheckPendingReloadEvent>
{
    private readonly IMediator _mediator;
    private readonly ILogger _logger;
    private readonly PendingReloadState _pendingReload;

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
            _logger.LogInformation(
                "Running System Reload"
            );

            await _mediator.Publish(
                new AdminCommandEvent(
                    BuildAdminCommand.FromString(
                        "reload-system"
                    ),
                    "reload-system"
                )
            );

            _pendingReload.RemovePending();
        }
    }
}
