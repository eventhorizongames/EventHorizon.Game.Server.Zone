namespace EventHorizon.Zone.System.Combat.Events.Level
{
    using MediatR;

    public struct LevelUpFailedEvent
        : INotification
    {
        public long EntityId { get; set; }
    }
}
