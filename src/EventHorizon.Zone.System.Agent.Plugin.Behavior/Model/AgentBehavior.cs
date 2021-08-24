namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Model
{
    using global::System;

    public struct AgentBehavior
    {
        public static readonly string PROPERTY_NAME = "behavior";

        public bool IsEnabled { get; set; }
        public string TreeId { get; set; }
        public DateTime NextTickRequest { get; set; }

        public object? this[string index]
        {
            get
            {
                return index switch
                {
                    "isEnabled" => IsEnabled,
                    "treeId" => TreeId,
                    "nextTickRequest" => NextTickRequest,
                    _ => null,
                };
            }
            set
            {
                switch (index)
                {
                    case "isEnabled":
                        IsEnabled = (bool?)value ?? false;
                        break;
                    case "treeId":
                        TreeId = (string?)value ?? "DEFAULT";
                        break;
                    case "nextTickRequest":
                        NextTickRequest = (DateTime?)value ?? DateTime.MinValue;
                        break;

                    default:
                        break;
                }
            }
        }
        public static readonly AgentBehavior NEW = new()
        {
            IsEnabled = true,
            TreeId = "DEFAULT",
            NextTickRequest = DateTime.MinValue,
        };
    }
}
