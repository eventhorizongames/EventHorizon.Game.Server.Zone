using System.Collections.Generic;
using System.Numerics;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Agent.Move
{
    public struct QueueAgentToMoveEvent : INotification
    {
        public long EntityId { get; set; }
        public Queue<Vector3> Path { get; set; }
    }
}