using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Events.Admin;
using EventHorizon.Zone.System.EntityModule.Load;
using MediatR;

namespace EventHorizon.Zone.System.EntityModule.Admin
{
    public class CommandReloadSystemHandler : INotificationHandler<AdminCommandReloadSystemEvent>
    {
        readonly IMediator _mediator;
        public CommandReloadSystemHandler(
            IMediator mediator
        )
        {
            _mediator = mediator;
        }
        public Task Handle(AdminCommandReloadSystemEvent notification, CancellationToken cancellationToken)
        {
            return _mediator.Publish(new LoadEntityModuleSystemCommand());
        }
    }
}