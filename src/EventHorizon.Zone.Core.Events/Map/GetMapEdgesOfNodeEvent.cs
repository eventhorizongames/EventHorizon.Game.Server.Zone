namespace EventHorizon.Zone.Core.Events.Map
{
    using System.Collections.Generic;
    using EventHorizon.Zone.Core.Model.Map;
    using MediatR;

    public struct GetMapEdgesOfNodeEvent : IRequest<IEnumerable<MapEdge>>
    {
        public int NodeIndex { get; }

        public GetMapEdgesOfNodeEvent(
            int nodeIndex
        )
        {
            NodeIndex = nodeIndex;
        }
    }
}