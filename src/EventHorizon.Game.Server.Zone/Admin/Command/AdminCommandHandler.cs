using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Admin.Command.Model;
using EventHorizon.Game.Server.Zone.Events.Admin;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventHorizon.Game.Server.Zone.Admin.Command
{
    public class AdminCommandHandler : IRequestHandler<AdminCommandEvent, AdminCommandResponse>
    {
        readonly ILogger _logger;
        readonly IMediator _mediator;
        public AdminCommandHandler(
            ILogger<AdminCommandHandler> logger,
            IMediator mediator
        )
        {
            _logger = logger;
            _mediator = mediator;
        }
        public async Task<AdminCommandResponse> Handle(AdminCommandEvent request, CancellationToken cancellationToken)
        {
            switch (request.Command)
            {
                case "reload-system":
                    _logger.LogInformation("Running Full System Reload...");
                    await _mediator.Publish(new AdminCommandReloadSystemEvent { Data = request.Data });
                    return new AdminCommandResponse
                    {
                        Success = true
                    };
                default:
                    return new AdminCommandResponse { Success = false, Message = "command_not_found" };
            }
        }
    }
}