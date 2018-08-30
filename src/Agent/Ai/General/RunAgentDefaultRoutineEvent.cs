using MediatR;

namespace EventHorizon.Game.Server.Zone.Agent.Ai.General
{
    public class RunAgentDefaultRoutineEvent : INotification
    {
        public long AgentId { get; set; }
    }
}