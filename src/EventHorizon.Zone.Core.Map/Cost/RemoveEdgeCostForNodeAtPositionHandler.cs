namespace EventHorizon.Zone.Core.Map.Cost
{
    using System.Threading;
    using System.Threading.Tasks;
    using EventHorizon.Zone.Core.Events.Map;
    using EventHorizon.Zone.Core.Events.Map.Cost;
    using EventHorizon.Zone.Core.Model.Map;
    using MediatR;

    public class RemoveEdgeCostForNodeAtPositionHandler : IRequestHandler<RemoveEdgeCostForNodeAtPosition, bool>
    {
        public readonly IMediator _mediator;
        public readonly IMapGraph _map;
        
        public RemoveEdgeCostForNodeAtPositionHandler(
            IMediator mediator,
            IMapGraph map
        )
        {
            _mediator = mediator;
            _map = map;
        }
        
        public async Task<bool> Handle(
            RemoveEdgeCostForNodeAtPosition request,
            CancellationToken cancellationToken
        )
        {
            // Lookup node if not Mapped
            var node = request.IsNode ? request.Node : await _mediator.Send(
                new GetMapNodeAtPositionEvent
                {
                    Position = request.Position
                }
            );
            await _mediator.Send(
                new UpdateDensityAndCostDetailsForNode(
                    node,
                    -1,
                    -request.Cost
                )
            );

            return true;
        }
    }
}