namespace EventHorizon.Zone.System.Agent.Plugin.Companion.Model
{
    public struct CompanionState
    {
        public static readonly string PROPERTY_NAME = "companionState";

        public string DefaultBehaviorTreeId { get; set; }

        public object this[string index]
        {
            get
            {
                switch (index)
                {
                    case "defaultBehaviorTreeId":
                        return this.DefaultBehaviorTreeId;

                    default:
                        return null;
                }
            }
            set
            {
                switch (index)
                {
                    case "defaultBehaviorTreeId":
                        this.DefaultBehaviorTreeId = (string)value;
                        break;

                    default:
                        break;
                }
            }
        }
    }
}
