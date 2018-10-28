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

            HealthPoints = 10,
            MaxHealthPoints = 10,

            ActionPoints = 10,
            MaxActionPoints = 10,

            Attack = 1,
        };
    }
}