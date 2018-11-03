using MediatR;

namespace EventHorizon.Game.Server.Zone.Events.Entity.Movement
{
    public struct StopEntityMovementEvent : INotification
    {
        public long EntityId { get; set; }
    }
}