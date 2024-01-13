namespace EventHorizon.Zone.System.Combat.Model.Level;

public struct LevelProperty
{
    public static readonly LevelProperty HP = new("HP");
    public static readonly LevelProperty AP = new("AP");
    public static readonly LevelProperty ATTACK = new("Attack");

    public string Property { get; }

    public LevelProperty(string property)
    {
        Property = property;
    }
}
