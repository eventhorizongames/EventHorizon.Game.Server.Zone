using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Admin.Command.Scripts.Load;
using EventHorizon.Zone.Core.Events.Admin.Command;
using EventHorizon.Zone.Core.Events.Admin.Command.Respond;
using EventHorizon.Zone.Core.Model.Admin;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Admin.Command.Reload
{
    public struct ReloadAdminCommandsHandler : INotificationHandler<AdminCommandEvent>
    {
        readonly IMediator _mediator;
        public ReloadAdminCommandsHandler(
            IMediator mediator
        )
        {
            _mediator = mediator;
        }
        public async Task Handle(
            AdminCommandEvent notification,
            CancellationToken cancellationToken
        )
        {
            if (notification.Command.Command != "reload-admin")
            {
                return;
            }
            await _mediator
                    .Send(new LoadAdminCommands());
            await _mediator.Send(
                new ResponseToAdminCommand(
                    notification.ConnectionId,
                    new StandardAdminCommandResponse(
                        notification.Command.Command,
                        notification.Command.RawCommand,
                        true,
                        "reload_admin_successful"
                    )
                )
            );
        }
    }
}