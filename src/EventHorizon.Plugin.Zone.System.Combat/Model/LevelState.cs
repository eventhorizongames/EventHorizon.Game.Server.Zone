namespace EventHorizon.Plugin.Zone.System.Combat.Model
{
    public struct LevelState
    {
        public static readonly string PROPERTY_NAME = "LevelState";

        public long HealthPointsLevel { get; set; }
        public long ActionPointsLevel { get; set; }
        public long AttackLevel { get; set; }

        public long Experience { get; set; }
        public long AllTimeExperience { get; set; }

        public object this [string index]
        {
            get
            {
                switch (index)
                {
                    case "HealthPointsLevel":
                        return this.HealthPointsLevel;
                    case "ActionPointsLevel":
                        return this.ActionPointsLevel;
                    case "AttackLevel":
                        return this.AttackLevel;

                    case "Experience":
                        return this.Experience;
                    case "AllTimeExperience":
                        return this.AllTimeExperience;

                    default:
                        return null;
                }
            }
            set
            {
                switch (index)
                {
                    case "HealthPointsLevel":
                        this.HealthPointsLevel = (long) value;
                        break;
                    case "ActionPointsLevel":
                        this.ActionPointsLevel = (long) value;
                        break;
                    case "AttackLevel":
                        this.AttackLevel = (long) value;
                        break;

                    case "Experience":
                        this.Experience = (long) value;
                        break;
                    case "AllTimeExperience":
                        this.AllTimeExperience = (long) value;
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