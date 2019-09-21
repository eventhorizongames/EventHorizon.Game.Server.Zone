using EventHorizon.Zone.Core.Model.Map;
using MediatR;

namespace EventHorizon.Zone.Core.Events.Map
{
    public class GetMapNodeAtIndexEvent : IRequest<MapNode>
    {
        public int NodeIndex { get; set; }   
    }
}