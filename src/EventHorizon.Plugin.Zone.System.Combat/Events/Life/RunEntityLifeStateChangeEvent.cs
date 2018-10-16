using EventHorizon.Plugin.Zone.System.Combat.Model.Life;
using MediatR;

namespace EventHorizon.Plugin.Zone.System.Combat.Events.Life
{
    public struct RunEntityLifeStateChangeEvent : INotification
    {
        public long EntityId { get; set; }
        public LifeProperty Property { get; set; }
        public int Points { get; set; }
    }
}