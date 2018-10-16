namespace EventHorizon.Plugin.Zone.System.Combat.Model
{
    public struct LevelState
    {
        public static readonly string PROPERTY_NAME = "LevelState";

        public int HealthPointsLevel { get; set; }
        public int ActionPointsLevel { get; set; }
        public int AttackLevel { get; set; }

        public int Experience { get; set; }
        public int AllTimeExperience { get; set; }

        public static readonly LevelState NEW = new LevelState
        {
            HealthPointsLevel = 1,

            ActionPointsLevel = 1,

            AttackLevel = 1,

            Experience = 0,
            AllTimeExperience = 3,
        };
    }
}