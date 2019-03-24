using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Admin.SystemWatcher.State;
using EventHorizon.Game.Server.Zone.Events.Admin;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Admin.SystemWatcher.Check
{
    public struct CheckForSystemReloadHandler : INotificationHandler<CheckForSystemReloadEvent>
    {
        readonly IMediator _mediator;
        readonly ISystemWatcherState _systemWatcherState;

        public CheckForSystemReloadHandler(
            IMediator mediator,
            ISystemWatcherState systemWatcherState
        )
        {
            _mediator = mediator;
            _systemWatcherState = systemWatcherState;
        }

        public async Task Handle(CheckForSystemReloadEvent notification, CancellationToken cancellationToken)
        {
            if (_systemWatcherState.PendingReload)
            {
                await _mediator.Publish(
                    new AdminCommandReloadSystemEvent()
                );
                _systemWatcherState.RemovePendingReload();
            }
        }
    }
}