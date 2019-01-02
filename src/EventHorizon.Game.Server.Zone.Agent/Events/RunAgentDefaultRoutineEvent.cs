using MediatR;

namespace EventHorizon.Game.Server.Zone.Agent.Events
{
    public struct RunAgentDefaultRoutineEvent : INotification
    {
        public long EntityId { get; set; }
    }
}