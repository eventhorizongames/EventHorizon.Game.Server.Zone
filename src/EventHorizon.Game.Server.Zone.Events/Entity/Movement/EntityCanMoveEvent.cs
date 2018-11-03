using MediatR;

namespace EventHorizon.Game.Server.Zone.Events.Entity.Movement
{
    public struct EntityCanMoveEvent : INotification
    {
        public long EntityId { get; set; }
    }
}