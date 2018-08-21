using MediatR;

namespace EventHorizon.Game.Server.Zone.Agent.Ai
{
    public class ClearAgentRoutineEvent : INotification
    {
        public long AgentId { get; set; }
    }
}