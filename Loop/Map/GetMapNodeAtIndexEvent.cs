using System.Numerics;
using EventHorizon.Game.Server.Zone.Map;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Loop.Map
{
    public class GetMapNodeAtIndexEvent : IRequest<MapNode>
    {
        public int NodeIndex { get; set; }   
    }
}