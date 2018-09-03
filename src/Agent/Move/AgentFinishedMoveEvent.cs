using MediatR;

namespace EventHorizon.Game.Server.Zone.Agent.Move
{
    public struct AgentFinishedMoveEvent : INotification
    {
        public long AgentId { get; set; }
    }
}