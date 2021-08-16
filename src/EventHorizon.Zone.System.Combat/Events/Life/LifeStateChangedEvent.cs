using MediatR;

namespace EventHorizon.Zone.System.Combat.Events.Life
{
    public struct LifeStateChangedEvent : INotification
    {
        public long EntityId { get; internal set; }
    }
}
