namespace EventHorizon.Zone.Core.ServerAction.Model;

using System;

using MediatR;

public struct ServerActionEntity
{
    public Guid _guid;
    public DateTime RunAt { get; private set; }
    public INotification EventToSend { get; private set; }

    public ServerActionEntity(
        DateTime runAt,
        INotification eventToSend
    )
    {
        _guid = Guid.NewGuid();
        RunAt = runAt;
        EventToSend = eventToSend;
    }
}
