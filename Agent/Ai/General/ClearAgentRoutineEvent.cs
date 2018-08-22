using MediatR;

namespace EventHorizon.Game.Server.Zone.Agent.Ai.General
{
    public class ClearAgentRoutineEvent : INotification
    {
        public long AgentId { get; set; }
    }
}