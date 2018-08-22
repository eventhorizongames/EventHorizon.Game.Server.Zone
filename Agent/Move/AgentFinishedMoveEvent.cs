using MediatR;

namespace EventHorizon.Game.Server.Zone.Agent.Move
{
    public class AgentFinishedMoveEvent : INotification
    {
        public long AgentId { get; set; }
    }
}