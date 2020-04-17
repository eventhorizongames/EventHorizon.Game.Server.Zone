namespace EventHorizon.Zone.Core.Events.Path
{
    using System.Collections.Generic;
    using System.Numerics;
    using MediatR;

    public struct FindPathEvent : IRequest<Queue<Vector3>>
    {
        public Vector3 From { get; set; }
        public Vector3 To { get; set; }

        public FindPathEvent(
            Vector3 from,
            Vector3 to
        )
        {
            From = from;
            To = to;
        }
    }
}