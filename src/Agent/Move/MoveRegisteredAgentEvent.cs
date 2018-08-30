using MediatR;

namespace EventHorizon.Game.Server.Zone.Agent.Move
{
    public class MoveRegisteredAgentEvent : INotification
    {
        public long AgentId { get; set; }
    }
}