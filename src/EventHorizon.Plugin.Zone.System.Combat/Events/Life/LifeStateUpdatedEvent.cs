using MediatR;

namespace EventHorizon.Plugin.Zone.System.Combat.Events.Life
{
    public struct LifeStateUpdatedEvent : INotification
    {
        public long EntityId { get; internal set; }
    }
}