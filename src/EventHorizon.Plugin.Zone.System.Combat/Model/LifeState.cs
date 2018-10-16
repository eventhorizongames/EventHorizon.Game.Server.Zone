using EventHorizon.Plugin.Zone.System.Combat.Model.Life;

namespace EventHorizon.Plugin.Zone.System.Combat.Model
{
    public struct LifeState
    {
        public static readonly string PROPERTY_NAME = "LifeState";

        public LifeCondition Condition { get; set; }

        public int HealthPoints { get; set; }
        public int MaxHealthPoints { get; set; }

        public int ActionPoints { get; set; }
        public int MaxActionPoints { get; set; }

        public int Attack { get; set; }

        public static readonly LifeState NEW = new LifeState
        {
            Condition = LifeCondition.ALIVE,

            HealthPoints = 10,
            MaxHealthPoints = 10,

            ActionPoints = 10,
            MaxActionPoints = 10,

            Attack = 1,
        };
    }
}