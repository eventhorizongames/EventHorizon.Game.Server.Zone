namespace EventHorizon.Zone.System.Combat.Model.Level;

public struct EntityLevelUp
{
    public static readonly EntityLevelUp NULL = default;

    public long EntityId { get; set; }
    public LevelProperty Property { get; set; }
}
