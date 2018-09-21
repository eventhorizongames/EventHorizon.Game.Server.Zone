using MediatR;

namespace EventHorizon.Game.Server.Zone.Agent.Ai.General
{
    public struct SetAgentRoutineEvent : INotification
    {
        public long AgentId { get; set; }
        public AiRoutine Routine { get; set; }
    }
}