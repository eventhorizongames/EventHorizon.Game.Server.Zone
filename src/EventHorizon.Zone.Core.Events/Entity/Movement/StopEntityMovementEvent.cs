using MediatR;

namespace EventHorizon.Zone.Core.Events.Entity.Movement
{
    public struct StopEntityMovementEvent : INotification
    {
        public long EntityId { get; set; }
    }
}