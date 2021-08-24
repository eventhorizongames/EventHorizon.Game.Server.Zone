namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Model
{
    using global::System.Collections.Generic;

    public class SerializedBehaviorNode
    {
        public string Type { get; set; } = "ACTION";
        public string? Status { get; set; }
        public string Fire { get; set; } = string.Empty;
        public int FailGate { get; set; }
        public bool Reset { get; set; }
        public IList<SerializedBehaviorNode>? NodeList { get; set; }
    }
}
