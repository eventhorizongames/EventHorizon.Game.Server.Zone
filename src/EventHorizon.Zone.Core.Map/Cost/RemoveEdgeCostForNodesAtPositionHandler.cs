namespace EventHorizon.Zone.Core.Map.Cost
{
    using System.Threading;
    using System.Threading.Tasks;
    using EventHorizon.Zone.Core.Events.Map;
    using EventHorizon.Zone.Core.Events.Map.Cost;
    using MediatR;

    public class RemoveEdgeCostForNodesAtPositionHandler : IRequestHandler<RemoveEdgeCostForNodesAtPosition, bool>
    {
        readonly IMediator _mediator;
        
        public RemoveEdgeCostForNodesAtPositionHandler(
            IMediator mediator
        )
        {
            _mediator = mediator;
        }

        public async Task<bool> Handle(
            RemoveEdgeCostForNodesAtPosition request,
            CancellationToken cancellationToken
        )
        {
            // Lookup node.
            var nodeList = await _mediator.Send(
                new GetMapNodesInDimensionsCommand(
                    request.Position,
                    request.BoundingBox
                )
            );
            
            foreach (var node in nodeList)
            {
                await _mediator.Send(
                    new RemoveEdgeCostForNodeAtPosition(
                        node,
                        request.Cost
                    )
                );
            }

            return true;
        }
    }
}