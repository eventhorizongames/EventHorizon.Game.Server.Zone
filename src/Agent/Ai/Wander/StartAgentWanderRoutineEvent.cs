using MediatR;

namespace EventHorizon.Game.Server.Zone.Agent.Ai.Wander
{
    public struct StartAgentWanderRoutineEvent : INotification
    {
        public long AgentId { get; set; }
    }
}