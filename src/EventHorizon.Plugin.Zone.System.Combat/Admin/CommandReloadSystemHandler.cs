using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Events.Admin;
using EventHorizon.Plugin.Zone.System.Combat.Load;
using MediatR;

namespace EventHorizon.Zone.System.Combat.Admin
{
    public class CommandReloadCombatSystemHandler : INotificationHandler<AdminCommandReloadSystemEvent>
    {
        readonly IMediator _mediator;
        public CommandReloadCombatSystemHandler(
            IMediator mediator
        )
        {
            _mediator = mediator;
        }
        public Task Handle(AdminCommandReloadSystemEvent notification, CancellationToken cancellationToken)
        {
            return _mediator.Publish(
                new LoadCombatSystemEvent()
            );
        }
    }
}