using MediatR;

namespace EventHorizon.Plugin.Zone.System.Combat.Events.Level
{
    public struct LevelStateUpdatedEvent : INotification
    {
        public long EntityId { get; set; }
    }
}