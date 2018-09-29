using EventHorizon.Game.Server.Zone.Agent.Model;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Agent.Events
{
    public struct SetAgentRoutineEvent : INotification
    {
        public long AgentId { get; set; }
        public AgentRoutine Routine { get; set; }
    }
}