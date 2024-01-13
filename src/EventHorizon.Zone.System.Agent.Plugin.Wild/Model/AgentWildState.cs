namespace EventHorizon.Zone.System.Agent.Plugin.Wild.Model;

public struct AgentWildState
{
    public static readonly string PROPERTY_NAME = "agentWildState";

    public int DistanceToRunAway { get; set; }
    public int DeltaDistance { get; set; }

    public object? this[string index]
    {
        get
        {
            return index switch
            {
                "distanceToRunAway" => DistanceToRunAway,
                "deltaDistance" => DeltaDistance,
                _ => null,
            };
        }
        set
        {
            switch (index)
            {
                case "distanceToRunAway":
                    DistanceToRunAway = (int?)value ?? 25;
                    break;
                case "deltaDistance":
                    DeltaDistance = (int?)value ?? 5;
                    break;

                default:
                    break;
            }
        }
    }

    public static readonly AgentWildState NEW = new()
    {
        DistanceToRunAway = 25,
        DeltaDistance = 5,
    };
}
