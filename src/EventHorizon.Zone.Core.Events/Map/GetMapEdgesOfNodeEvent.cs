using System.Collections.Generic;
using EventHorizon.Zone.Core.Model.Map;
using MediatR;

namespace EventHorizon.Zone.Core.Events.Map
{
    public class GetMapEdgesOfNodeEvent : IRequest<IEnumerable<MapEdge>>
    {
        public int NodeIndex { get; set; }   
    }
}