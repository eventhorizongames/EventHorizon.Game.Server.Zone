using EventHorizon.Game.Server.Zone.Agent.Model.Ai;
using EventHorizon.Game.Server.Zone.Core.Model;

namespace EventHorizon.Game.Server.Zone.Agent.Model
{
    public struct AgentDetails
    {
        public string Name { get; set; }
        public PositionState Position { get; set; }

        public float Speed { get; set; }
        public AgentAiState Ai { get; set; }
        public dynamic Data { get; set; }
    }
}