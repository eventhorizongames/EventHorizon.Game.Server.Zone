using System.Collections.Generic;
using EventHorizon.Game.Server.Zone.Model.Map;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Events.Map
{
    public class GetMapEdgesOfNodeEvent : IRequest<IEnumerable<MapEdge>>
    {
        public int NodeIndex { get; set; }   
    }
}