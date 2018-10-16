namespace EventHorizon.Plugin.Zone.System.Combat.Model.Life
{
    public struct ChangeEntityLife
    {
        public static ChangeEntityLife NULL = default(ChangeEntityLife);

        public long EntityId { get; set; }
        public LifeProperty Property { get; set; }
        public int Points { get; set; }
    }
}