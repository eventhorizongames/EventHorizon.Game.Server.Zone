using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Admin.Command.Model;
using EventHorizon.Game.Server.Zone.Admin.Command.ReloadCombatSystem;
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
        public Task<AdminCommandResponse> Handle(AdminCommandEvent request, CancellationToken cancellationToken)
        {
            switch (request.Command)
            {
                case "reload-combat-system":
                    return _mediator.Send(new AdminCommandReloadCombatSystemEvent { Data = request.Data });
                case "reload-system":
                    _logger.LogInformation("Running Full System Reload...");
                    _mediator.Publish(new AdminCommandReloadSystemEvent { Data = request.Data }).ConfigureAwait(false);
                    return Task.FromResult(
                        new AdminCommandResponse
                        {
                            Success = true
                        }
                    );
                default:
                    return Task.FromResult(
                        new AdminCommandResponse { Success = false, Message = "command_not_found" }
                    );
            }
        }
    }
}