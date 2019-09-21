namespace EventHorizon.Zone.System.Combat.Model.Life
{
    public struct ChangeEntityLife
    {
        public static ChangeEntityLife NULL = default(ChangeEntityLife);

        public long EntityId { get; set; }
        public string Property { get; set; }
        public long Points { get; set; }
    }
}