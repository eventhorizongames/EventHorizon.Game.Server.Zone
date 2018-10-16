using MediatR;

namespace EventHorizon.Plugin.Zone.System.Combat.Events.Life
{
    public struct LifeStateChangedEvent : INotification
    {
        public long EntityId { get; internal set; }
    }
}