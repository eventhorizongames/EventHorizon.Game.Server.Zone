namespace EventHorizon.Zone.System.Combat.Model;

using EventHorizon.Zone.System.Combat.Model.Life;

using global::System;

public struct LifeState
{
    public static readonly string PROPERTY_NAME = "lifeState";

    public LifeCondition Condition { get; set; }

    public long HealthPoints { get; set; }
    public long MaxHealthPoints { get; set; }

    public long ActionPoints { get; set; }
    public long MaxActionPoints { get; set; }

    public long Attack { get; set; }

    public object? this[string index]
    {
        get
        {
            return index switch
            {
                "condition" => Condition,
                "healthPoints" => HealthPoints,
                "maxHealthPoints" => MaxHealthPoints,
                "actionPoints" => ActionPoints,
                "maxActionPoints" => MaxActionPoints,
                "attack" => Attack,
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
                case "condition":
                    Condition = (LifeCondition)value;
                    break;

                case "healthPoints":
                    HealthPoints = (int)value;
                    break;
                case "maxHealthPoints":
                    MaxHealthPoints = (int)value;
                    break;

                case "actionPoints":
                    ActionPoints = (int)value;
                    break;
                case "maxActionPoints":
                    MaxActionPoints = (int)value;
                    break;

                case "attack":
                    Attack = (int)value;
                    break;

                default:
                    break;
            }
        }
    }


    public static readonly LifeState NEW = new()
    {
        Condition = LifeCondition.ALIVE,

        HealthPoints = 100,
        MaxHealthPoints = 100,

        ActionPoints = 100,
        MaxActionPoints = 100,

        Attack = 1,
    };
}
