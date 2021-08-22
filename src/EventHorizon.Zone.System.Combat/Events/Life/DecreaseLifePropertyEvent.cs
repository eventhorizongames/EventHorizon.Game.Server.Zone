namespace EventHorizon.Zone.System.Combat.Events.Life
{
    using MediatR;

    public struct DecreaseLifePropertyEvent
        : INotification
    {
        public long EntityId { get; set; }
        public string Property { get; set; }
        public long Points { get; set; }
    }
}
