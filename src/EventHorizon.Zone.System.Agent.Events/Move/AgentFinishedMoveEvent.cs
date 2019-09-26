using MediatR;

namespace EventHorizon.Zone.System.Agent.Events.Move
{
    public struct AgentFinishedMoveEvent : INotification
    {
        public long EntityId { get; set; }
    }
}