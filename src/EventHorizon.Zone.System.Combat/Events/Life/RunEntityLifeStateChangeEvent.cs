using MediatR;

namespace EventHorizon.Zone.System.Combat.Events.Life
{
    public struct RunEntityLifeStateChangeEvent : INotification
    {
        public long EntityId { get; set; }
        public string Property { get; set; }
        public long Points { get; set; }
    }
}