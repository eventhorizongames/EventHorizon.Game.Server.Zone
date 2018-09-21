using MediatR;

namespace EventHorizon.Game.Server.Zone.Agent.Ai.General
{
    public struct RunAgentDefaultRoutineEvent : INotification
    {
        public long AgentId { get; set; }
    }
}