namespace EventHorizon.Plugin.Zone.System.Combat.Model.Level
{
    public struct EntityLevelUp
    {
        public static readonly EntityLevelUp NULL = default(EntityLevelUp);

        public long EntityId { get; set; }
        public LevelProperty Property { get; set; }
    }
}