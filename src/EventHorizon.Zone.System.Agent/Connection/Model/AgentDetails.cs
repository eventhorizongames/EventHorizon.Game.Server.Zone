using System.Collections.Generic;
using EventHorizon.Zone.Core.Model.Core;

namespace EventHorizon.Zone.System.Agent.Connection.Model
{
    public struct AgentDetails
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public PositionState Position { get; set; }
        public IList<string> TagList { get; set; }
        public Dictionary<string, object> Data { get; set; }
    }
}