namespace EventHorizon.Zone.System.Admin.ExternalHub.AdminClientAction;

using EventHorizon.Zone.System.Admin.AdminClientAction.Client;
using EventHorizon.Zone.System.Admin.ExternalHub;

using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

using Microsoft.AspNetCore.SignalR;

public class SendToSingleAdminClientEventHandler
    : INotificationHandler<SendToSingleAdminClientEvent>
{
    private readonly IHubContext<AdminHub> _hubContext;

    public SendToSingleAdminClientEventHandler(
        IHubContext<AdminHub> hubContext
    )
    {
        _hubContext = hubContext;
    }

    public async Task Handle(
        SendToSingleAdminClientEvent notification,
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
