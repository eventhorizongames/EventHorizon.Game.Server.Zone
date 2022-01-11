namespace EventHorizon.Zone.System.ClientEntities.Update;

using EventHorizon.Zone.Core.Events.Map.Cost;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.ClientEntities.Model;

using global::System.Numerics;
using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

public class SetClientEntityNodeDensityHandler
    : IRequestHandler<SetClientEntityNodeDensity>
{
    private readonly IMediator _mediator;

    public SetClientEntityNodeDensityHandler(
        IMediator mediator
    )
    {
        _mediator = mediator;
    }

    public async Task<Unit> Handle(
        SetClientEntityNodeDensity request,
        CancellationToken cancellationToken
    )
    {
        var entity = request.ClientEntity;
        // At position if they are dense, increase cost to get to node
        if (entity.ContainsProperty(
            nameof(ClientEntityMetadataTypes.TYPE_DETAILS.dense)
        ) && entity.GetProperty<bool>(
            nameof(ClientEntityMetadataTypes.TYPE_DETAILS.dense)
        ))
        {
            if (entity.ContainsProperty(
                nameof(ClientEntityMetadataTypes.TYPE_DETAILS.densityBox)
            ))
            {
                await _mediator.Send(
                    new ChangeEdgeCostForNodesAtPositionCommand(
                        entity.Transform.Position,
                        entity.GetProperty<Vector3>(
                            nameof(ClientEntityMetadataTypes.TYPE_DETAILS.densityBox)
                        ),
                        500
                    ),
                    cancellationToken
                );
                return Unit.Value;
            }
            else
            {
                await _mediator.Send(
                    new ChangeEdgeCostForNodeAtPosition(
                        entity.Transform.Position,
                        500
                    ),
                    cancellationToken
                );
            }
        }
        return Unit.Value;
    }
}
