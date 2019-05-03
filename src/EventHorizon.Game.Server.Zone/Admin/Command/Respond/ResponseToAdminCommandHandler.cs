using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Admin.Bus;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace EventHorizon.Game.Server.Zone.Admin.Command.Respond
{
    public struct ResponseToAdminCommandHandler : IRequestHandler<ResponseToAdminCommand, bool>
    {
        readonly IHubContext<AdminBus> _hubContext;
        public ResponseToAdminCommandHandler(
            IHubContext<AdminBus> hubContext
        )
        {
            _hubContext = hubContext;
        }
        public async Task<bool> Handle(
            ResponseToAdminCommand request,
            CancellationToken cancellationToken
        )
        {
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