using MediatR;

namespace EventHorizon.Game.Server.Zone.Agent.Events
{
    public struct ClearAgentRoutineEvent : INotification
    {
        public long AgentId { get; set; }
    }
}