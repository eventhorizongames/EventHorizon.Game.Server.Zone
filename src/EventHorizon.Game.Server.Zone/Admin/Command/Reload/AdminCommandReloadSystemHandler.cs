using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Admin.Command.Model;
using EventHorizon.Game.Server.Zone.Admin.Command.Respond;
using EventHorizon.Game.Server.Zone.Events.Admin;
using EventHorizon.Game.Server.Zone.Model.Admin;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventHorizon.Game.Server.Zone.Admin.Command.Reload
{
    public class AdminCommandReloadSystemHandler : INotificationHandler<AdminCommandEvent>
    {
        readonly ILogger _logger;
        readonly IMediator _mediator;
        public AdminCommandReloadSystemHandler(
            ILogger<AdminCommandReloadSystemHandler> logger,
            IMediator mediator
        )
        {
            _logger = logger;
            _mediator = mediator;
        }
        public async Task Handle(
            AdminCommandEvent request,
            CancellationToken cancellationToken
        )
        {
            if (request.Command.Command != "reload-system")
            {
                return;
            }
            _logger.LogInformation("Running Full System Reload...");
            await _mediator.Publish(
                new AdminCommandReloadSystemEvent
                {
                    Data = request.Data
                }
            );
            await _mediator.Send(
                new ResponseToAdminCommand(
                    request.ConnectionId,
                    new StandardAdminCommandResponse(
                        request.Command.Command,
                        request.Command.RawCommand,
                        true,
                        "reload_system_success"
                    )
                )
            );

        }
    }
}