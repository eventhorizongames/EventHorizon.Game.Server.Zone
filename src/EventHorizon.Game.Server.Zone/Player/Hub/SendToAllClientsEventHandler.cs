namespace EventHorizon.Game.Server.Zone.Player.Bus;

using System.Threading;
using System.Threading.Tasks;

using EventHorizon.Zone.Core.Events.Client;
using EventHorizon.Zone.System.Player.ExternalHub;

using MediatR;

using Microsoft.AspNetCore.SignalR;

public class SendToAllClientsEventHandler
    : INotificationHandler<SendToAllClientsEvent>
{
    private readonly IHubContext<PlayerHub> _hubContext;

    public SendToAllClientsEventHandler(
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
                notification.Arg2,
                cancellationToken: cancellationToken
            );
    }
}
