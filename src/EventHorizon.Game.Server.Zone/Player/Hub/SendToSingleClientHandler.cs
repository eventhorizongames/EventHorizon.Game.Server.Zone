namespace EventHorizon.Game.Server.Zone.Player.Bus
{

    using System.Threading;
    using System.Threading.Tasks;

    using EventHorizon.Zone.Core.Events.Client;
    using EventHorizon.Zone.System.Player.ExternalHub;

    using MediatR;

    using Microsoft.AspNetCore.SignalR;

    public class SendToSingleClientHandler : INotificationHandler<SendToAllClientsEvent>
    {
        readonly IHubContext<PlayerHub> _hubContext;
        public SendToSingleClientHandler(
            IHubContext<PlayerHub> hubContext
        )
        {
            _hubContext = hubContext;
        }

        public async Task Handle(
            SendToAllClientsEvent notification,
            CancellationToken cancellationToken
        )
        {
            await _hubContext.Clients.All
                .SendAsync(
                    notification.Method,
                    notification.Arg1,
                    notification.Arg2
                );
        }
    }
}
