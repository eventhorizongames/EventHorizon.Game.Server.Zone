using MediatR;

namespace EventHorizon.Game.Server.Zone.Agent.Move
{
    public struct MoveRegisteredAgentEvent : INotification
    {
        public long EntityId { get; set; }
    }
}