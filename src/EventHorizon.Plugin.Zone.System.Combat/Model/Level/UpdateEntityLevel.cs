namespace EventHorizon.Plugin.Zone.System.Combat.Model.Level
{
    public struct UpdateEntityLevel
    {
        public static readonly UpdateEntityLevel NULL = default(UpdateEntityLevel);

        public int EntityId { get; set; }
        public LevelProperty Property { get; set; }
    }
}