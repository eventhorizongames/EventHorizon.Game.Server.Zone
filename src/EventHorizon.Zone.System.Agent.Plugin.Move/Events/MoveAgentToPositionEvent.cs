using System.Numerics;
using MediatR;

namespace EventHorizon.Zone.System.Agent.Plugin.Move.Events
{
    public struct MoveAgentToPositionEvent : IRequest
    {
        public long AgentId { get; set; }
        public Vector3 ToPosition { get; set; }
    }
}