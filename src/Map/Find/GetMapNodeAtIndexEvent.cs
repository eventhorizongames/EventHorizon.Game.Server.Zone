using System.Numerics;
using EventHorizon.Game.Server.Zone.Map.Model;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Map
{
    public class GetMapNodeAtIndexEvent : IRequest<MapNode>
    {
        public int NodeIndex { get; set; }   
    }
}