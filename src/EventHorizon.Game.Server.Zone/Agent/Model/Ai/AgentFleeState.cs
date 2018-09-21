using System.Collections.Generic;
using EventHorizon.Game.Server.Zone.Agent.Ai;

namespace EventHorizon.Game.Server.Zone.Agent.Model.Ai
{
    public struct AgentFleeState
    {
        public int SightDistance { get; set; }
        public float DistanceToRun { get; set; }
        public AiRoutine FallbackRoutine { get; set; }
        public IList<string> TagList { get; set; }
    }
}