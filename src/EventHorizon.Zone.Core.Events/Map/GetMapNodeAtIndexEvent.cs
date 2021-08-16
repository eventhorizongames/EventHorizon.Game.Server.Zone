namespace EventHorizon.Zone.Core.Events.Map
{
    using EventHorizon.Zone.Core.Model.Map;

    using MediatR;

    public struct GetMapNodeAtIndexEvent : IRequest<MapNode>
    {
        public int NodeIndex { get; set; }

        public GetMapNodeAtIndexEvent(
            int nodeIndex
        )
        {
            NodeIndex = nodeIndex;
        }
    }
}
