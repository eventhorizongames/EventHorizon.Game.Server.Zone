using System.Collections.Generic;
using System.Numerics;
using EventHorizon.Game.Server.Zone.Map;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Loop.Map
{
    public struct GetMapNodesAroundPositionEvent : IRequest<IList<MapNode>>
    {
        public Vector3 Position { get; set; }   
        public int Distance { get; set; }
    }
}