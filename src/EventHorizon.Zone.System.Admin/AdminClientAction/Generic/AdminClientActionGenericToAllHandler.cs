namespace EventHorizon.Zone.System.Admin.AdminClientAction.Generic
{
    using EventHorizon.Zone.System.Admin.AdminClientAction.Model;

    using MediatR;

    public class AdminClientActionGenericToAllHandler
        : AdminClientActionToAllHandler<AdminClientActionGenericToAllEvent, IAdminClientActionData>,
            INotificationHandler<AdminClientActionGenericToAllEvent>
    {
        public AdminClientActionGenericToAllHandler(
            IMediator mediator
        ) : base(mediator)
        {
        }
    }
}
