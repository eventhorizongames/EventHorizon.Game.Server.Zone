namespace EventHorizon.Zone.System.Admin.AdminClientAction.Generic;

using EventHorizon.Zone.System.Admin.AdminClientAction.Model;

using MediatR;

public class AdminClientActionGenericToSingleHandler
    : AdminClientActionToSingleHandler<AdminClientActionGenericToSingleEvent, IAdminClientActionData>,
        INotificationHandler<AdminClientActionGenericToSingleEvent>
{
    public AdminClientActionGenericToSingleHandler(
        IMediator mediator
    ) : base(mediator)
    {
    }
}
