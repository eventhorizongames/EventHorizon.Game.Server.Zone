using MediatR;

namespace EventHorizon.Game.Server.Zone.Agent.Ai.General
{
    public class SetAgentRoutineEvent : INotification
    {
        public long AgentId { get; set; }
        public AiRoutine Routine { get; set; }
    }
}