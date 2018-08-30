using System.Numerics;
using EventHorizon.Game.Server.Zone.Map;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Loop.Map
{
    public class GetMapNodeAtPositionEvent : IRequest<MapNode>
    {
        public Vector3 Position { get; set; }   
    }
}