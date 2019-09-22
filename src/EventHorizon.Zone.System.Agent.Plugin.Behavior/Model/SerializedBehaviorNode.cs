using System.Collections.Generic;

namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Model
{
    public class SerializedBehaviorNode
    {
        public string Type { get; set; }
        public string Status { get; set; }
        public string Fire { get; set; }
        public int FailGate { get; set; }
        public IList<SerializedBehaviorNode> NodeList { get; set; }
    }
}