using MediatR;

namespace EventHorizon.Game.Server.Zone.Agent.Ai.Flee
{
    public struct StartAgentFleeRoutineEvent : INotification
    {
        public long AgentId { get; set; }
    }
}