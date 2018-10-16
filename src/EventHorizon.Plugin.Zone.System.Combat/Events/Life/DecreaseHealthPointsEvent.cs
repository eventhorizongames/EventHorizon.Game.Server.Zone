using MediatR;

namespace EventHorizon.Plugin.Zone.System.Combat.Events.Life
{
    public struct DecreaseHealthPointsEvent : INotification
    {
        public long EntityId { get; set; }
        public int Points { get; set; }
    }
}