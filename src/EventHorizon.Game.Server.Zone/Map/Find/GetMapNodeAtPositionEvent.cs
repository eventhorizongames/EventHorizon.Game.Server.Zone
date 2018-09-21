using System.Numerics;
using EventHorizon.Game.Server.Zone.Map.Model;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Map
{
    public struct GetMapNodeAtPositionEvent : IRequest<MapNode>
    {
        public Vector3 Position { get; set; }   
    }
}