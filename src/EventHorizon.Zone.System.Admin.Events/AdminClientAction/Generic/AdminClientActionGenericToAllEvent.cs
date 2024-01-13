namespace EventHorizon.Zone.System.Admin.AdminClientAction.Generic;

using EventHorizon.Zone.System.Admin.AdminClientAction.Client;
using EventHorizon.Zone.System.Admin.AdminClientAction.Model;

using MediatR;

public class AdminClientActionGenericToAllEvent
    : AdminClientActionToAllEvent<IAdminClientActionData>,
    INotification
{
    public override string Action { get; }
    public override IAdminClientActionData Data { get; }

    public AdminClientActionGenericToAllEvent(
        string action,
        IAdminClientActionData data
    )
    {
        Action = action;
        Data = data;
    }
}
