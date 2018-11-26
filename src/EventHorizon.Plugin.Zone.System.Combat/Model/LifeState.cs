using EventHorizon.Plugin.Zone.System.Combat.Model.Life;

namespace EventHorizon.Plugin.Zone.System.Combat.Model
{
    public struct LifeState
    {
        public static readonly string PROPERTY_NAME = "LifeState";

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
                    case "Condition":
                        return this.Condition;

                    case "HealthPoints":
                        return this.HealthPoints;
                    case "MaxHealthPoints":
                        return this.MaxHealthPoints;

                    case "ActionPoints":
                        return this.ActionPoints;
                    case "MaxActionPoints":
                        return this.MaxActionPoints;

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
                    case "Condition":
                        this.Condition = (LifeCondition)value;
                        break;

                    case "HealthPoints":
                        this.HealthPoints = (int)value;
                        break;
                    case "MaxHealthPoints":
                        this.MaxHealthPoints = (int)value;
                        break;

                    case "ActionPoints":
                        this.ActionPoints = (int)value;
                        break;
                    case "MaxActionPoints":
                        this.MaxActionPoints = (int)value;
                        break;

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