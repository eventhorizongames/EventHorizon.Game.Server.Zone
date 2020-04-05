namespace EventHorizon.Zone.Core.Map.Cost
{
    using System.Threading;
    using System.Threading.Tasks;
    using EventHorizon.Zone.Core.Events.Map;
    using EventHorizon.Zone.Core.Events.Map.Cost;
    using MediatR;

    public class ChangeEdgeCostForNodesAtPositionCommandHandler : IRequestHandler<ChangeEdgeCostForNodesAtPositionCommand, bool>
    {
        readonly IMediator _mediator;

        public ChangeEdgeCostForNodesAtPositionCommandHandler(
            IMediator mediator
        )
        {
            _mediator = mediator;
        }

        public async Task<bool> Handle(
            ChangeEdgeCostForNodesAtPositionCommand request,
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
                    new ChangeEdgeCostForNodeAtPositionCommand(
                        node,
                        request.Cost
                    )
                );
            }

            return true;
        }
    }
}