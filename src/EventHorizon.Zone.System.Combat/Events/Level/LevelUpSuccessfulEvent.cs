namespace EventHorizon.Zone.System.Combat.Events.Level
{
    using MediatR;

    public struct LevelUpSuccessfulEvent
        : INotification
    {
        public long EntityId { get; set; }
    }
}
