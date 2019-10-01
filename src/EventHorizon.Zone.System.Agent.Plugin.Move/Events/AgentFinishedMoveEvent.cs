using MediatR;

namespace EventHorizon.Zone.System.Agent.Plugin.Move.Events
{
    public struct AgentFinishedMoveEvent : INotification
    {
        public long EntityId { get; set; }
    }
}