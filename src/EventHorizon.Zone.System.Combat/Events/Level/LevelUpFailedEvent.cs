using MediatR;

namespace EventHorizon.Zone.System.Combat.Events.Level
{
    public struct LevelUpFailedEvent : INotification
    {
        public long EntityId { get; set; }
    }
}
