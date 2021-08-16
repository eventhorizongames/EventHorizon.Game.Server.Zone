namespace EventHorizon.Zone.Core.Map.Cost
{
    using System.Threading;
    using System.Threading.Tasks;

    using EventHorizon.Zone.Core.Events.Map.Cost;

    using MediatR;

    public class RemoveEdgeCostForNodeHandler
        : IRequestHandler<RemoveEdgeCostForNode, bool>
    {
        private readonly IMediator _mediator;

        public RemoveEdgeCostForNodeHandler(
            IMediator mediator
        )
        {
            _mediator = mediator;
        }

        public async Task<bool> Handle(
            RemoveEdgeCostForNode request,
            CancellationToken cancellationToken
        )
        {
            // Lookup node if not Mapped
            await _mediator.Send(
                new UpdateDensityAndCostDetailsForNode(
                    request.Node,
                    -1,
                    -request.Cost
                )
            );

            return true;
        }
    }
}
