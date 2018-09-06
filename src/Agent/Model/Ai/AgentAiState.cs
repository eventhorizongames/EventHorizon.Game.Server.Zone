using EventHorizon.Game.Server.Zone.Agent.Ai;

namespace EventHorizon.Game.Server.Zone.Agent.Model.Ai
{
    public struct AgentAiState
    {
        public AiRoutine DefaultRoutine { get; set; }
        public AgentWanderState Wander { get; set; }
    }
}