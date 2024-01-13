namespace EventHorizon.Zone.Core.Map.Cost;

using System.Threading;
using System.Threading.Tasks;

using EventHorizon.Zone.Core.Events.Map;
using EventHorizon.Zone.Core.Events.Map.Cost;

using MediatR;

public class RemoveEdgeCostForNodeAtPositionHandler
    : IRequestHandler<RemoveEdgeCostForNodeAtPosition, bool>
{
    private readonly IMediator _mediator;

    public RemoveEdgeCostForNodeAtPositionHandler(
        IMediator mediator
    )
    {
        _mediator = mediator;
    }

    public async Task<bool> Handle(
        RemoveEdgeCostForNodeAtPosition request,
        CancellationToken cancellationToken
    )
    {
        var node = await _mediator.Send(
            new GetMapNodeAtPositionEvent(
                request.Position
            )
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
