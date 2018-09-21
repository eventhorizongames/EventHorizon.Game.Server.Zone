using MediatR;

namespace EventHorizon.Game.Server.Zone.Agent.Ai.General
{
    public struct ClearAgentRoutineEvent : INotification
    {
        public long AgentId { get; set; }
    }
}