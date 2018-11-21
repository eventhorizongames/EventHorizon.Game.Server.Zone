
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Client.DataType;
using EventHorizon.Game.Server.Zone.Events.Client;
using EventHorizon.Game.Server.Zone.Events.Client.Actions;
using EventHorizon.Game.Server.Zone.Model.Client;
using EventHorizon.Game.Server.Zone.Player;
using EventHorizon.Game.Server.Zone.Player.State;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace EventHorizon.Game.Server.Zone.Player.Bus
{
    public class SendToSingleClientHandler : INotificationHandler<SendToAllClientsEvent>
    {
        readonly IHubContext<PlayerHub> _hubContext;
        public SendToSingleClientHandler(IHubContext<PlayerHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task Handle(SendToAllClientsEvent notification, CancellationToken cancellationToken)
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