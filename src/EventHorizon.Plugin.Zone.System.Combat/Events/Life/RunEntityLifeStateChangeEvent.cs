using EventHorizon.Plugin.Zone.System.Combat.Model.Life;
using MediatR;

namespace EventHorizon.Plugin.Zone.System.Combat.Events.Life
{
    public struct RunEntityLifeStateChangeEvent : INotification
    {
        public long EntityId { get; set; }
        public string Property { get; set; }
        public long Points { get; set; }
    }
}