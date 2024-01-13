namespace EventHorizon.Zone.Core.Client.Generic;

using EventHorizon.Zone.Core.Client.Action;
using EventHorizon.Zone.Core.Events.Client.Generic;
using EventHorizon.Zone.Core.Model.Client;

using MediatR;

public class ClientActionGenericToSingleHandler
    : ClientActionToSingleHandler<ClientActionGenericToSingleEvent, IClientActionData>,
        INotificationHandler<ClientActionGenericToSingleEvent>
{
    public ClientActionGenericToSingleHandler(
        IMediator mediator
    ) : base(mediator)
    {
    }
}
