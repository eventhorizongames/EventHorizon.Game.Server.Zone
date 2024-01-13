namespace EventHorizon.Zone.Core.Model.Entity.Movement;

public struct MovementState
{
    public static readonly string PROPERTY_NAME = "movementState";

    public float Speed { get; set; }

    public object? this[string index]
    {
        get
        {
            return index switch
            {
                "speed" => Speed,
                _ => null,
            };
        }
        set
        {
            switch (index)
            {
                case "speed":
                    Speed = (float?)value ?? 1.0f;
                    break;

                default:
                    break;
            }
        }
    }

    public static readonly MovementState NEW = new()
    {
        Speed = 1.0f,
    };
}
