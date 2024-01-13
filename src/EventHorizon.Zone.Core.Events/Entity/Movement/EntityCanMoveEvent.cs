namespace EventHorizon.Zone.Core.Events.Entity.Movement;

using MediatR;

public struct EntityCanMoveEvent : INotification
{
    public long EntityId { get; set; }
}
