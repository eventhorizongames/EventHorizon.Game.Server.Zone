using System.Collections.Generic;
using EventHorizon.Game.Server.Zone.Agent.Ai;
using EventHorizon.Game.Server.Zone.Agent.Model;

namespace EventHorizon.Game.Server.Zone.Agent.Ai.Flee.Model.Ai
{
    public struct AgentFleeState
    {
        public int SightDistance { get; set; }
        public float DistanceToRun { get; set; }
        public AgentRoutine FallbackRoutine { get; set; }
        public IList<string> TagList { get; set; }
    }
}