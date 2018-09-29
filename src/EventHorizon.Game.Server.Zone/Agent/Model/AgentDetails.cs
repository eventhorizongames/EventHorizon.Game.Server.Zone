using System.Collections.Generic;
using EventHorizon.Game.Server.Zone.Core.Model;
using EventHorizon.Game.Server.Zone.Model.Core;

namespace EventHorizon.Game.Server.Zone.Agent.Model
{
    public struct AgentDetails
    {
        public string Name { get; set; }
        public PositionState Position { get; set; }
        public IList<string> TagList { get; set; }

        public float Speed { get; set; }
        public Dictionary<string, object> Data { get; set; }
    }
}