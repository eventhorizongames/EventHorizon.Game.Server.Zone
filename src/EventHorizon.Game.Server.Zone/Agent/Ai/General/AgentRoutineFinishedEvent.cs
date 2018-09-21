using MediatR;

namespace EventHorizon.Game.Server.Zone.Agent.Ai.General
{
    public struct AgentRoutineFinishedEvent : INotification
    {
        public long AgentId { get; set; }
    }
}