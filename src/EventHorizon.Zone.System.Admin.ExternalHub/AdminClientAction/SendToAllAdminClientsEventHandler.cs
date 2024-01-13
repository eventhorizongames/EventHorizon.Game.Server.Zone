namespace EventHorizon.Zone.System.Admin.ExternalHub.AdminClientAction;

using EventHorizon.Zone.System.Admin.AdminClientAction.Client;

using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

using Microsoft.AspNetCore.SignalR;

public class SendToAllAdminClientsEventHandler
    : INotificationHandler<SendToAllAdminClientsEvent>
{
    private readonly IHubContext<AdminHub> _hubContext;

    public SendToAllAdminClientsEventHandler(
        IHubContext<AdminHub> hubContext
    )
    {
        _hubContext = hubContext;
    }

    public async Task Handle(
        SendToAllAdminClientsEvent notification,
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
