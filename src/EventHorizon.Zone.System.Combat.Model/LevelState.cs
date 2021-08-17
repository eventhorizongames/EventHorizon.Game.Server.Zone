namespace EventHorizon.Zone.System.Combat.Model
{
    using global::System;

    public struct LevelState
    {
        public static readonly string PROPERTY_NAME = "levelState";

        public long HealthPointsLevel { get; set; }
        public long ActionPointsLevel { get; set; }
        public long AttackLevel { get; set; }

        public long Experience { get; set; }
        public long AllTimeExperience { get; set; }

        public object? this[string index]
        {
            get
            {
                return index switch
                {
                    "healthPointsLevel" => HealthPointsLevel,
                    "actionPointsLevel" => ActionPointsLevel,
                    "attackLevel" => AttackLevel,
                    "experience" => Experience,
                    "allTimeExperience" => AllTimeExperience,
                    _ => null,
                };
            }
            set
            {
                if (value is null)
                {
                    throw new ArgumentNullException(
                        $"{index} cannot be set to null"
                    );
                }

                switch (index)
                {
                    case "healthPointsLevel":
                        HealthPointsLevel = (long)value;
                        break;
                    case "actionPointsLevel":
                        ActionPointsLevel = (long)value;
                        break;
                    case "attackLevel":
                        AttackLevel = (long)value;
                        break;

                    case "experience":
                        Experience = (long)value;
                        break;
                    case "allTimeExperience":
                        AllTimeExperience = (long)value;
                        break;

                    default:
                        break;
                }
            }
        }

        public static readonly LevelState NEW = new()
        {
            HealthPointsLevel = 1,

            ActionPointsLevel = 1,

            AttackLevel = 1,

            Experience = 0,
            AllTimeExperience = 3,
        };
    }
}
