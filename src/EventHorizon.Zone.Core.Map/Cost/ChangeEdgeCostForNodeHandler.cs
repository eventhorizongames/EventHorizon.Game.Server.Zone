namespace EventHorizon.Zone.Core.Map.Cost
{
    using System.Threading;
    using System.Threading.Tasks;
    using EventHorizon.Zone.Core.Events.Map.Cost;
    using MediatR;

    public class ChangeEdgeCostForNodeHandler 
        : IRequestHandler<ChangeEdgeCostForNode, bool>
    {
        private readonly IMediator _mediator;

        public ChangeEdgeCostForNodeHandler(
            IMediator mediator
        )
        {
            _mediator = mediator;
        }

        public async Task<bool> Handle(
            ChangeEdgeCostForNode request,
            CancellationToken cancellationToken
        )
        {
            await _mediator.Send(
                new UpdateDensityAndCostDetailsForNode(
                    request.Node,
                    1,
                    request.Cost
                )
            );

            return true;
        }
    }
}