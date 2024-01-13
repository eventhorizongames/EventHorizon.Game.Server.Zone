namespace EventHorizon.Zone.System.Combat.Events.Level;

using EventHorizon.Zone.System.Combat.Model.Level;

using MediatR;

public struct RunLevelUpForEntityEvent
    : INotification
{
    public long EntityId { get; set; }
    public LevelProperty Property { get; set; }
}
