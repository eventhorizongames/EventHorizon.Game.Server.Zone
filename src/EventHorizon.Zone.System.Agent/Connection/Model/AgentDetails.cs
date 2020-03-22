namespace EventHorizon.Zone.System.Agent.Connection.Model
{
    using global::System.Collections.Generic;
    using EventHorizon.Zone.Core.Model.Core;

    public class AgentDetails
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public TransformState Transform { get; set;  }
        public LocationState Location { get; set; }
        public IList<string> TagList { get; set; }
        public Dictionary<string, object> Data { get; set; }
    }
}