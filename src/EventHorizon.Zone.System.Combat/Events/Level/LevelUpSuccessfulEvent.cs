using MediatR;

namespace EventHorizon.Zone.System.Combat.Events.Level
{
    public struct LevelUpSuccessfulEvent : INotification
    {
        public long EntityId { get; set; }
    }
}
