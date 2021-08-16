namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Model
{
    using global::System;

    public struct AgentBehavior
    {
        public static readonly string PROPERTY_NAME = "behavior";

        public bool IsEnabled { get; set; }
        public string TreeId { get; set; }
        public DateTime NextTickRequest { get; set; }

        public object this[string index]
        {
            get
            {
                switch (index)
                {
                    case "isEnabled":
                        return this.IsEnabled;
                    case "treeId":
                        return this.TreeId;
                    case "nextTickRequest":
                        return this.NextTickRequest;

                    default:
                        return null;
                }
            }
            set
            {
                switch (index)
                {
                    case "isEnabled":
                        this.IsEnabled = (bool)value;
                        break;
                    case "treeId":
                        this.TreeId = (string)value;
                        break;
                    case "nextTickRequest":
                        this.NextTickRequest = (DateTime)value;
                        break;

                    default:
                        break;
                }
            }
        }
        public static readonly AgentBehavior NEW = new AgentBehavior
        {
            IsEnabled = true,
            TreeId = "DEFAULT",
            NextTickRequest = DateTime.MinValue,
        };
    }
}
