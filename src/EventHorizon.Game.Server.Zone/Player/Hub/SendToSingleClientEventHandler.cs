namespace EventHorizon.Game.Server.Zone.Player.Bus;

using System.Threading;
using System.Threading.Tasks;

using EventHorizon.Zone.Core.Events.Client;
using EventHorizon.Zone.System.Player.ExternalHub;

using MediatR;

using Microsoft.AspNetCore.SignalR;

public class SendToSingleClientEventHandler
    : INotificationHandler<SendToSingleClientEvent>
{
    private readonly IHubContext<PlayerHub> _hubContext;

    public SendToSingleClientEventHandler(
        IHubContext<PlayerHub> hubContext
    )
    {
        _hubContext = hubContext;
    }

    public async Task Handle(
        SendToSingleClientEvent notification,
        CancellationToken cancellationToken
    )
    {
        if (string.IsNullOrEmpty(
            notification.ConnectionId
        ))
        {
            return;
        }

        await _hubContext
            .Clients
            .Client(
                notification.ConnectionId
            ).SendAsync(
                notification.Method,
                notification.Arg1,
                notification.Arg2,
                cancellationToken: cancellationToken
            );
    }
}
