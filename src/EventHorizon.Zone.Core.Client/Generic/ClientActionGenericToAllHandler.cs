namespace EventHorizon.Zone.Core.Client.Generic
{
    using EventHorizon.Zone.Core.Client.Action;
    using EventHorizon.Zone.Core.Events.Client.Generic;
    using EventHorizon.Zone.Core.Model.Client;

    using MediatR;

    public class ClientActionGenericToAllHandler
        : ClientActionToAllHandler<ClientActionGenericToAllEvent, IClientActionData>,
            INotificationHandler<ClientActionGenericToAllEvent>
    {
        public ClientActionGenericToAllHandler(
            IMediator mediator
        ) : base(mediator)
        {
        }
    }
}
