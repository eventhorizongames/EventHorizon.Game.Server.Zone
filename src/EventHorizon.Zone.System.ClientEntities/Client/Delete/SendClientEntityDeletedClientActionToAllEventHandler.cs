namespace EventHorizon.Zone.System.ClientEntities.Client.Delete
{
    using EventHorizon.Zone.Core.Client.Action;
    using EventHorizon.Zone.System.ClientEntities.Model.Client;
    using MediatR;

    public class SendClientEntityDeletedClientActionToAllEventHandler
        : ClientActionToAllHandler<SendClientEntityDeletedClientActionToAllEvent, ClientEntityDeletedClientActionData>,
            INotificationHandler<SendClientEntityDeletedClientActionToAllEvent>
    {
        public SendClientEntityDeletedClientActionToAllEventHandler(
            IMediator mediator
        ) : base(mediator)
        {
        }
    }
}