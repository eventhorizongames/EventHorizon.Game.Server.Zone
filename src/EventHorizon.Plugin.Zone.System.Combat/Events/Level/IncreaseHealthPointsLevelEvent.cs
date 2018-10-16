using MediatR;

namespace EventHorizon.Plugin.Zone.System.Combat.Events.Level
{
    public struct IncreaseHealthPointsLevelEvent : INotification
    {
        public long EntityId { get; set; }
        public int Points { get; set; }
    }
}