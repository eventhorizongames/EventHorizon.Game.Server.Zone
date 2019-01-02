namespace EventHorizon.Plugin.Zone.System.Combat.Model
{
    public struct LevelState
    {
        public static readonly string PROPERTY_NAME = "levelState";

        public long HealthPointsLevel { get; set; }
        public long ActionPointsLevel { get; set; }
        public long AttackLevel { get; set; }

        public long Experience { get; set; }
        public long AllTimeExperience { get; set; }

        public object this[string index]
        {
            get
            {
                switch (index)
                {
                    case "healthPointsLevel":
                        return this.HealthPointsLevel;
                    case "actionPointsLevel":
                        return this.ActionPointsLevel;
                    case "attackLevel":
                        return this.AttackLevel;

                    case "experience":
                        return this.Experience;
                    case "allTimeExperience":
                        return this.AllTimeExperience;

                    default:
                        return null;
                }
            }
            set
            {
                switch (index)
                {
                    case "healthPointsLevel":
                        this.HealthPointsLevel = (long)value;
                        break;
                    case "actionPointsLevel":
                        this.ActionPointsLevel = (long)value;
                        break;
                    case "attackLevel":
                        this.AttackLevel = (long)value;
                        break;

                    case "experience":
                        this.Experience = (long)value;
                        break;
                    case "allTimeExperience":
                        this.AllTimeExperience = (long)value;
                        break;

                    default:
                        break;
                }
            }
        }

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