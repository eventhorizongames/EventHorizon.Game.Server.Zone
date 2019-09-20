using System.Collections.Generic;
using EventHorizon.Game.Server.Zone.Core.Model;
using EventHorizon.Zone.Core.Model.Core;

namespace EventHorizon.Game.Server.Zone.Agent.Model
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