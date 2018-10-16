namespace EventHorizon.Plugin.Zone.System.Combat.Model.Life
{
    public struct UpdateEntityLife
    {
        public static UpdateEntityLife NULL = default(UpdateEntityLife);

        public long EntityId { get; set; }
        public LifeProperty Property { get; set; }
        public int Points { get; set; }
    }
}