using EventHorizon.Game.Server.Zone.Agent.Model.Data;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Agent.Ai.Wander
{
    public class StartAgentWanderRoutineEvent : INotification
    {
        public long AgentId { get; set; }
        public AgentWanderData Wander { get; set; }
    }
}