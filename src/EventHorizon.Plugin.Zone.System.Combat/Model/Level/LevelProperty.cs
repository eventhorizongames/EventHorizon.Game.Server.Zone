namespace EventHorizon.Plugin.Zone.System.Combat.Model.Level
{
    public struct LevelProperty
    {
        public static readonly LevelProperty HP = new LevelProperty("HP");
        public static readonly LevelProperty AP = new LevelProperty("AP");
        public static readonly LevelProperty ATTACK = new LevelProperty("Attack");

        public string Property { get; }

        public LevelProperty(string property)
        {
            Property = property;
        }
    }
}