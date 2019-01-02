using MediatR;

namespace EventHorizon.Game.Server.Zone.Agent.Events
{
    public struct AgentRoutineFinishedEvent : INotification
    {
        public long EntityId { get; set; }
    }
}