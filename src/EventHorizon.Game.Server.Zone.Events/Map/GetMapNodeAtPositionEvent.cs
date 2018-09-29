using System.Numerics;
using EventHorizon.Game.Server.Zone.Model.Map;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Events.Map
{
    public struct GetMapNodeAtPositionEvent : IRequest<MapNode>
    {
        public Vector3 Position { get; set; }   
    }
}