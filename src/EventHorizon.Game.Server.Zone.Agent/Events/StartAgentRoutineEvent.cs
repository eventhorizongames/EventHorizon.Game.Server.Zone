using System;
using EventHorizon.Game.Server.Zone.Agent.Model;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Agent.Events
{
    public struct StartAgentRoutineEvent : INotification
    {
        public AgentRoutine Routine { get; set; }
        public long EntityId { get; set; }
    }
}