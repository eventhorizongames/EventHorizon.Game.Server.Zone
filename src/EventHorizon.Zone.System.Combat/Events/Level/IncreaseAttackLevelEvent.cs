namespace EventHorizon.Zone.System.Combat.Events.Level
{
    using MediatR;

    public struct IncreaseAttackLevelEvent
        : INotification
    {
        public long EntityId { get; set; }
        public int Points { get; set; }
    }
}
