using System.Collections.Generic;
using System.Numerics;
using MediatR;

namespace EventHorizon.Zone.System.Agent.Events.Move
{
    public struct QueueAgentToMoveEvent : INotification
    {
        public long EntityId { get; set; }
        public Queue<Vector3> Path { get; set; }
    }
}