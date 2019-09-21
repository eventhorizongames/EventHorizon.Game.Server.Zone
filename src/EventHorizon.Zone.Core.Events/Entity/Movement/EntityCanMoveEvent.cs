using MediatR;

namespace EventHorizon.Zone.Core.Events.Entity.Movement
{
    public struct EntityCanMoveEvent : INotification
    {
        public long EntityId { get; set; }
    }
}