using System;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Agent.Ai.General
{
    public struct StartAgentRoutineEvent : INotification
    {
        public AiRoutine Routine { get; set; }
        public long AgentId { get; set; }
    }
}