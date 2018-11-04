using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Admin.Command.Model;
using EventHorizon.Game.Server.Zone.Admin.Command.ReloadCombatSystem;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Admin.Command
{
    public class AdminCommandHandler : IRequestHandler<AdminCommandEvent, AdminCommandResponse>
    {
        readonly IMediator _mediator;
        public AdminCommandHandler(
            IMediator mediator
        )
        {
            _mediator = mediator;
        }
        public Task<AdminCommandResponse> Handle(AdminCommandEvent request, CancellationToken cancellationToken)
        {
            switch (request.Command)
            {
                case "reload-combat-system":
                    return _mediator.Send(new AdminCommandReloadCombatSystemEvent { Data = request.Data });
                default:
                    return Task.FromResult(
                        new AdminCommandResponse { Success = false, Message = "command_not_found" }
                    );
            }
        }
    }
}