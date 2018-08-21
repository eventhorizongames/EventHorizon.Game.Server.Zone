using MediatR;

namespace EventHorizon.Game.Server.Zone.Agent.Ai
{
    public class RunAgentDefaultRoutineEvent : INotification
    {
        public long AgentId { get; set; }
    }
}