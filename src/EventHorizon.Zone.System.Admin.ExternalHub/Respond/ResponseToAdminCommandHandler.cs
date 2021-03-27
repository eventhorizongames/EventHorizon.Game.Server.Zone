namespace EventHorizon.Zone.System.Admin.ExternalHub.Respond
{
    using EventHorizon.Zone.System.Admin.Plugin.Command.Events;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using MediatR;
    using Microsoft.AspNetCore.SignalR;

    public class RespondToAdminCommandHandler
        : IRequestHandler<RespondToAdminCommand, bool>
    {
        private readonly IHubContext<AdminHub> _hubContext;

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
            if (string.IsNullOrEmpty(
                request.ConnectionId
            ))
            {
                return false;
            }
            await _hubContext.Clients.Client(
                request.ConnectionId
            ).SendAsync(
                "AdminCommandResponse",
                request.Response,
                cancellationToken
            );
            return true;
        }
    }
}
