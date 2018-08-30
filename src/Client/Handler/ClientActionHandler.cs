using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Player;
using EventHorizon.Game.Server.Zone.Player.State;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace EventHorizon.Game.Server.Zone.Client.Handler
{
    public class ClientActionHandler : INotificationHandler<ClientActionEvent>
    {
        readonly IHubContext<PlayerHub> _hubContext;
        public ClientActionHandler(IHubContext<PlayerHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task Handle(ClientActionEvent notification, CancellationToken cancellationToken)
        {
            await _hubContext.Clients.All
                .SendAsync("ClientAction", notification.Action, notification.Data);
        }
    }
}