using EventHorizon.Plugin.Zone.System.Combat.Model.Level;
using MediatR;

namespace EventHorizon.Plugin.Zone.System.Combat.Events.Level
{
    public struct RunLevelUpForEntityEvent : INotification
    {
        public int EntityId { get; set; }
        public LevelProperty Property { get; set; }
    }
}