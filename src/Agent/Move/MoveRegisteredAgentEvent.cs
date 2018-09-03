using MediatR;

namespace EventHorizon.Game.Server.Zone.Agent.Move
{
    public struct MoveRegisteredAgentEvent : INotification
    {
        public long AgentId { get; set; }
    }
}