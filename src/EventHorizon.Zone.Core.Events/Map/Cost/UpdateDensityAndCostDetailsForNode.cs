namespace EventHorizon.Zone.Core.Events.Map.Cost
{
    using EventHorizon.Zone.Core.Model.Map;
    using MediatR;

    public struct UpdateDensityAndCostDetailsForNode : IRequest
    {
        public MapNode Node { get; }
        public int Dense { get; }
        public int Cost { get; }

        public UpdateDensityAndCostDetailsForNode(
            MapNode node,
            int dense,
            int cost
        )
        {
            Node = node;
            Dense = dense;
            Cost = cost;
        }
    }
}