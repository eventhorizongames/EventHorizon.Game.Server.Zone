namespace EventHorizon.Zone.System.ClientEntities.Client
{
    using EventHorizon.Zone.Core.Client.Action;
    using EventHorizon.Zone.System.ClientEntities.Model.Client;
    using MediatR;

    public class SendClientEntityChangedClientActionToAllEventHandler
        : ClientActionToAllHandler<SendClientEntityChangedClientActionToAllEvent, ClientEntityChangedClientActionData>,
            INotificationHandler<SendClientEntityChangedClientActionToAllEvent>
    {
        public SendClientEntityChangedClientActionToAllEventHandler(
            IMediator mediator
        ) : base(mediator)
        {
        }
    }
}