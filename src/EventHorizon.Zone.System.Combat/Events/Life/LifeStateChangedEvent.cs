namespace EventHorizon.Zone.System.Combat.Events.Life;

using MediatR;

public struct LifeStateChangedEvent
    : INotification
{
    public long EntityId { get; internal set; }
}
