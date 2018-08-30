using System.Collections.Generic;
using EventHorizon.Game.Server.Zone.Map;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Loop.Map
{
    public class GetMapEdgesOfNodeEvent : IRequest<IList<MapEdge>>
    {
        public int NodeIndex { get; set; }   
    }
}