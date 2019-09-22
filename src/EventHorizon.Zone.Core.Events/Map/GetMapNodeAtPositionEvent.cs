using System.Numerics;
using EventHorizon.Zone.Core.Model.Map;
using MediatR;

namespace EventHorizon.Zone.Core.Events.Map
{
    public struct GetMapNodeAtPositionEvent : IRequest<MapNode>
    {
        public Vector3 Position { get; set; }   
    }
}