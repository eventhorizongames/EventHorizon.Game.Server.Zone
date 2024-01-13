namespace EventHorizon.Zone.System.Admin.AdminClientAction.Generic;

using EventHorizon.Zone.System.Admin.AdminClientAction.Client;
using EventHorizon.Zone.System.Admin.AdminClientAction.Model;

using MediatR;

public class AdminClientActionGenericToSingleEvent 
    : AdminClientActionToSingleEvent<IAdminClientActionData>,
    INotification
{
    public override string ConnectionId { get; }
    public override string Action { get; }
    public override IAdminClientActionData Data { get; }

    public AdminClientActionGenericToSingleEvent(
        string connectionId,
        string action,
        IAdminClientActionData data
    )
    {
        ConnectionId = connectionId;
        Action = action;
        Data = data;
    }
}
