using MediatR;

namespace EventHorizon.Game.Server.Zone.Agent.Ai.General
{
    public class AgentRoutineFinishedEvent : INotification
    {
        public long AgentId { get; set; }
    }
}