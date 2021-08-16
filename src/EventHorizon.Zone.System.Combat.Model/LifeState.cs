using EventHorizon.Zone.System.Combat.Model.Life;

namespace EventHorizon.Zone.System.Combat.Model
{
    public struct LifeState
    {
        public static readonly string PROPERTY_NAME = "lifeState";

        public LifeCondition Condition { get; set; }

        public long HealthPoints { get; set; }
        public long MaxHealthPoints { get; set; }

        public long ActionPoints { get; set; }
        public long MaxActionPoints { get; set; }

        public long Attack { get; set; }

        public object this[string index]
        {
            get
            {
                switch (index)
                {
                    case "condition":
                        return this.Condition;

                    case "healthPoints":
                        return this.HealthPoints;
                    case "maxHealthPoints":
                        return this.MaxHealthPoints;

                    case "actionPoints":
                        return this.ActionPoints;
                    case "maxActionPoints":
                        return this.MaxActionPoints;

                    case "attack":
                        return this.Attack;

                    default:
                        return null;
                }
            }
            set
            {
                switch (index)
                {
                    case "condition":
                        this.Condition = (LifeCondition)value;
                        break;

                    case "healthPoints":
                        this.HealthPoints = (int)value;
                        break;
                    case "maxHealthPoints":
                        this.MaxHealthPoints = (int)value;
                        break;

                    case "actionPoints":
                        this.ActionPoints = (int)value;
                        break;
                    case "maxActionPoints":
                        this.MaxActionPoints = (int)value;
                        break;

                    case "attack":
                        this.Attack = (int)value;
                        break;

                    default:
                        break;
                }
            }
        }


        public static readonly LifeState NEW = new LifeState
        {
            Condition = LifeCondition.ALIVE,

            HealthPoints = 100,
            MaxHealthPoints = 100,

            ActionPoints = 100,
            MaxActionPoints = 100,

            Attack = 1,
        };
    }
}
