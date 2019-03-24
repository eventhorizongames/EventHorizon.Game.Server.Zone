using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.I18n.Loader;
using EventHorizon.Game.Server.Zone.Events.Admin;
using MediatR;

namespace EventHorizon.Game.I18n.Admin
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
        public Task Handle(
            AdminCommandReloadSystemEvent notification, 
            CancellationToken cancellationToken
        )
        {
            return _mediator.Publish(
                new I18nLoadEvent()
            );
        }
    }
}