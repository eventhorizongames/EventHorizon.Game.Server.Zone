using MediatR;

namespace EventHorizon.Plugin.Zone.System.Combat.Events
{
    public struct LowerEntityHealthEvent : INotification
    {
        public long EntityId { get; set; }
        public int PointsToDecrease { get; set; }
    }
}