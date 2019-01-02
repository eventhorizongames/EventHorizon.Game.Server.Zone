using MediatR;

namespace EventHorizon.Game.Server.Zone.Agent.Move
{
    public struct AgentFinishedMoveEvent : INotification
    {
        public long EntityId { get; set; }
    }
}