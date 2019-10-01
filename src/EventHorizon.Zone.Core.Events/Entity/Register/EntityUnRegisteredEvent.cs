using MediatR;

namespace EventHorizon.Zone.Core.Events.Entity.Register
{
    public struct EntityUnRegisteredEvent : INotification
    {
        public long EntityId { get; set; }
    }
}