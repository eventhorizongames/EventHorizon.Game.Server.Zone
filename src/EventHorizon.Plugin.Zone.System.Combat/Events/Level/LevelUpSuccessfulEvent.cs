using MediatR;

namespace EventHorizon.Plugin.Zone.System.Combat.Events.Level
{
    public struct LevelUpSuccessfulEvent : INotification
    {
        public long EntityId { get; set; }
    }
}