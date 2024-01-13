namespace EventHorizon.Zone.Core.Map.Cost;

using System.Threading;
using System.Threading.Tasks;

using EventHorizon.Zone.Core.Events.Map;
using EventHorizon.Zone.Core.Events.Map.Cost;

using MediatR;

public class ChangeEdgeCostForNodeAtPositionHandler
    : IRequestHandler<ChangeEdgeCostForNodeAtPosition, bool>
{
    private readonly IMediator _mediator;

    public ChangeEdgeCostForNodeAtPositionHandler(
        IMediator mediator
    )
    {
        _mediator = mediator;
    }

    public async Task<bool> Handle(
        ChangeEdgeCostForNodeAtPosition request,
        CancellationToken cancellationToken
    )
    {
        await _mediator.Send(
            new UpdateDensityAndCostDetailsForNode(
                await _mediator.Send(
                    new GetMapNodeAtPositionEvent(
                        request.Position
                    )
                ),
                1,
                request.Cost
            )
        );

        return true;
    }
}
