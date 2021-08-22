namespace EventHorizon.Zone.System.Agent.Plugin.Companion.Model
{
    public struct CompanionState
    {
        public static readonly string PROPERTY_NAME = "companionState";

        public string DefaultBehaviorTreeId { get; set; }

        public object? this[string index]
        {
            get
            {
                return index switch
                {
                    "defaultBehaviorTreeId" => DefaultBehaviorTreeId,
                    _ => null,
                };
            }
            set
            {
                switch (index)
                {
                    case "defaultBehaviorTreeId":
                        DefaultBehaviorTreeId = (string?)value ?? "DEFAULT";
                        break;

                    default:
                        break;
                }
            }
        }
    }
}
