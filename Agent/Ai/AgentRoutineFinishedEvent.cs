using MediatR;

namespace EventHorizon.Game.Server.Zone.Agent.Ai
{
    public class AgentRoutineFinishedEvent : INotification
    {
        public long AgentId { get; set; }
    }
}