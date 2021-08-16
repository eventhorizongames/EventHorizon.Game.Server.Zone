using System.Threading;
using System.Threading.Tasks;

using EventHorizon.Zone.System.Admin.Plugin.Command.Events;
using EventHorizon.Zone.System.Admin.Plugin.Command.Load;
using EventHorizon.Zone.System.Admin.Plugin.Command.Model.Standard;

using MediatR;

namespace EventHorizon.Zone.System.Admin.Plugin.Command.Reload
{
    public class ReloadAdminCommandsHandler : INotificationHandler<AdminCommandEvent>
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
            await _mediator.Send(
                new LoadAdminCommands()
            );
            await _mediator.Send(
                new RespondToAdminCommand(
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
