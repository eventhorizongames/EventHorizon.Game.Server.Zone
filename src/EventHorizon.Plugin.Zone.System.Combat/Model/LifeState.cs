using EventHorizon.Plugin.Zone.System.Combat.Model.Life;

namespace EventHorizon.Plugin.Zone.System.Combat.Model
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
                    case "Condition":
                        return this.Condition;

                    case "healthPoints":
                    case "HealthPoints":
                        return this.HealthPoints;
                    case "maxHealthPoints":
                    case "MaxHealthPoints":
                        return this.MaxHealthPoints;

                    case "actionPoints":
                    case "ActionPoints":
                        return this.ActionPoints;
                    case "maxActionPoints":
                    case "MaxActionPoints":
                        return this.MaxActionPoints;

                    case "attack":
                    case "Attack":
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
                    case "Condition":
                        this.Condition = (LifeCondition)value;
                        break;

                    case "healthPoints":
                    case "HealthPoints":
                        this.HealthPoints = (int)value;
                        break;
                    case "maxHealthPoints":
                    case "MaxHealthPoints":
                        this.MaxHealthPoints = (int)value;
                        break;

                    case "actionPoints":
                    case "ActionPoints":
                        this.ActionPoints = (int)value;
                        break;
                    case "maxActionPoints":
                    case "MaxActionPoints":
                        this.MaxActionPoints = (int)value;
                        break;

                    case "attack":
                    case "Attack":
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