using System.Collections.Generic;
using System.Numerics;
using MediatR;

namespace EventHorizon.Zone.Core.Events.Path
{
    public struct FindPathEvent : IRequest<Queue<Vector3>>
    {
        public Vector3 From { get; set; }
        public Vector3 To { get; set; }
    }
}