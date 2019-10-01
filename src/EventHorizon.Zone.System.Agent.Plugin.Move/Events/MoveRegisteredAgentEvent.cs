using MediatR;

namespace EventHorizon.Zone.System.Agent.Plugin.Move.Events
{
    public struct MoveRegisteredAgentEvent : INotification
    {
        public long EntityId { get; set; }
    }
}