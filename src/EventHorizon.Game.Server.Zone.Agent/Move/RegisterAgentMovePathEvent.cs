using System.Collections.Generic;
using System.Numerics;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Agent.Move
{
    public struct RegisterAgentMovePathEvent : INotification
    {
        public long EntityId { get; set; }
        public Queue<Vector3> Path { get; set; }
    }
}