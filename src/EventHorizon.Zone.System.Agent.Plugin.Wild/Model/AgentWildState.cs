namespace EventHorizon.Zone.System.Agent.Plugin.Wild.Model
{
    public struct AgentWildState
    {
        public static readonly string PROPERTY_NAME = "agentWildState";

        public int DistanceToRunAway { get; set; }
        public int DeltaDistance { get; set; }

        public object this[string index]
        {
            get
            {
                switch (index)
                {
                    case "distanceToRunAway":
                        return this.DistanceToRunAway;
                    case "deltaDistance":
                        return this.DeltaDistance;

                    default:
                        return null;
                }
            }
            set
            {
                switch (index)
                {
                    case "distanceToRunAway":
                        this.DistanceToRunAway = (int)value;
                        break;
                    case "deltaDistance":
                        this.DeltaDistance = (int)value;
                        break;

                    default:
                        break;
                }
            }
        }

        public static readonly AgentWildState NEW = new AgentWildState
        {
            DistanceToRunAway = 25,
            DeltaDistance = 5,
        };
    }
}
