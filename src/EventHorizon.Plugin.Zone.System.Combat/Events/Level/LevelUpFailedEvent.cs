using MediatR;

namespace EventHorizon.Plugin.Zone.System.Combat.Events.Level
{
    public struct LevelUpFailedEvent : INotification
    {
        public long EntityId { get; set; }
    }
}