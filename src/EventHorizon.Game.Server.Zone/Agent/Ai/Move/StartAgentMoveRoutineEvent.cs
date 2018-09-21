using System.Numerics;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Agent.Ai.Move
{
    public struct StartAgentMoveRoutineEvent : INotification
    {
        public long AgentId { get; set; }
        public Vector3 ToPosition { get; set; }
    }
}