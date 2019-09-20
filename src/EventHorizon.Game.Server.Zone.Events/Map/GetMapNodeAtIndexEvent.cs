using System.Numerics;
using EventHorizon.Zone.Core.Model.Map;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Events.Map
{
    public class GetMapNodeAtIndexEvent : IRequest<MapNode>
    {
        public int NodeIndex { get; set; }   
    }
}