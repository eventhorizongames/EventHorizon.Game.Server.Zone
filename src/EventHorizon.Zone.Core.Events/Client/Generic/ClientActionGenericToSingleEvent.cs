namespace EventHorizon.Zone.Core.Events.Client.Generic;

using EventHorizon.Zone.Core.Model.Client;

using MediatR;

public class ClientActionGenericToSingleEvent : ClientActionToSingleEvent<IClientActionData>, INotification
{
    public override string ConnectionId { get; }
    public override string Action { get; }
    public override IClientActionData Data { get; }

    public ClientActionGenericToSingleEvent(
        string connectionId,
        string action,
        IClientActionData data
    )
    {
        ConnectionId = connectionId;
        Action = action;
        Data = data;
    }
}
