using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Model.Client.DataType;
using EventHorizon.Zone.Core.Events.Client;
using EventHorizon.Zone.Core.Events.Client.Actions;
using EventHorizon.Zone.Core.Model.Client;
using EventHorizon.Game.Server.Zone.Player;
using EventHorizon.Game.Server.Zone.Player.State;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace EventHorizon.Game.Server.Zone.Player.Bus
{
    public class SendToAllClientsHandler : INotificationHandler<SendToSingleClientEvent>
    {
        readonly IHubContext<PlayerHub> _hubContext;
        public SendToAllClientsHandler(IHubContext<PlayerHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task Handle(SendToSingleClientEvent notification, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(notification.ConnectionId))
            {
                return;
            }
            await _hubContext
                .Clients
                .Client(notification.ConnectionId)
                .SendAsync(
                    notification.Method,
                    notification.Arg1,
                    notification.Arg2
                );
        }
    }
}