using MediatR;

namespace EventHorizon.Plugin.Zone.System.Combat.Events.Level
{
    public struct IncreaseActionPointsLevelEvent : INotification
    {
        public long EntityId { get; set; }
        public int Points { get; set; }
    }
}