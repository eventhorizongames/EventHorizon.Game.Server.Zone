using MediatR;

namespace EventHorizon.Zone.System.Agent.Events.Move
{
    public struct MoveRegisteredAgentEvent : INotification
    {
        public long EntityId { get; set; }
    }
}