namespace EventHorizon.Zone.System.Agent.Connection.Model
{
    using EventHorizon.Zone.Core.Model.Core;

    using global::System.Collections.Concurrent;
    using global::System.Collections.Generic;

    public class AgentDetails
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public TransformState Transform { get; set; }
        public LocationState Location { get; set; }
        public IList<string> TagList { get; set; }
        public ConcurrentDictionary<string, object> Data { get; set; }
    }
}
