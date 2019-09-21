using MediatR;

namespace EventHorizon.Zone.System.Combat.Events.Level
{
    public struct IncreaseExperienceEvent : INotification
    {
        public long EntityId { get; set; }
        public int Points { get; set; }
    }
}