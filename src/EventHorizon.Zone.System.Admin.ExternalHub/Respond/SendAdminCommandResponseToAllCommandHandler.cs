namespace EventHorizon.Zone.System.Admin.ExternalHub.Respond
{
    using EventHorizon.Zone.System.Admin.Plugin.Command.Events;

    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    using Microsoft.AspNetCore.SignalR;

    public class SendAdminCommandResponseToAllCommandHandler
        : IRequestHandler<SendAdminCommandResponseToAllCommand, bool>
    {
        private readonly IHubContext<AdminHub> _hubContext;

        public SendAdminCommandResponseToAllCommandHandler(
            IHubContext<AdminHub> hubContext
        )
        {
            _hubContext = hubContext;
        }

        public async Task<bool> Handle(
            SendAdminCommandResponseToAllCommand request,
            CancellationToken cancellationToken
        )
        {
            await _hubContext.Clients.All.SendAsync(
                "AdminCommandResponse",
                request.Response,
                cancellationToken
            );
            return true;
        }
    }
}
