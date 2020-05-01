namespace EventHorizon.Zone.Core.Events.Map.Cost
{
    using EventHorizon.Zone.Core.Model.Map;
    using MediatR;

    public struct ChangeEdgeCostForNode : IRequest<bool>
    {
        public MapNode Node { get; }
        public int Cost { get; }

        public ChangeEdgeCostForNode(
            MapNode node,
            int cost
        )
        {
            Node = node;
            Cost = cost;
        }
    }
}