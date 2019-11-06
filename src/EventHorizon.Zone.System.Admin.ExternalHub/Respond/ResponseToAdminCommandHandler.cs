using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Admin.Plugin.Command.Events;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace EventHorizon.Zone.System.Admin.ExternalHub.Respond
{
    public class RespondToAdminCommandHandler : IRequestHandler<RespondToAdminCommand, bool>
    {
        readonly IHubContext<AdminHub> _hubContext;
        public RespondToAdminCommandHandler(
            IHubContext<AdminHub> hubContext
        )
        {
            _hubContext = hubContext;
        }
        public async Task<bool> Handle(
            RespondToAdminCommand request,
            CancellationToken cancellationToken
        )
        {
            if(string.IsNullOrEmpty(
                request.ConnectionId
            )) {
                return false;
            }
            await _hubContext.Clients.Client(
                request.ConnectionId
            ).SendAsync(
                "AdminCommandResponse",
                request.Response
            );
            return true;
        }
    }
}