namespace EventHorizon.Zone.System.ClientEntities.Unregister
{
    using EventHorizon.Zone.Core.Events.Map.Cost;
    using EventHorizon.Zone.Core.Model.Entity;
    using EventHorizon.Zone.System.ClientEntities.Model;
    using EventHorizon.Zone.System.ClientEntities.State;

    using global::System.Numerics;
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    public class UnregisterClientEntityHandler : IRequestHandler<UnregisterClientEntity, bool>
    {
        private readonly IMediator _mediator;
        private readonly ClientEntityRepository _repository;

        public UnregisterClientEntityHandler(
            IMediator mediator,
            ClientEntityRepository repository
        )
        {
            _mediator = mediator;
            _repository = repository;
        }

        public async Task<bool> Handle(
            UnregisterClientEntity request,
            CancellationToken cancellationToken
        )
        {
            // Find existing entity from repository
            var entity = _repository.Find(
                request.GlobalId
            );
            if (!entity.IsFound())
            {
                return false;
            }
            // If Dense remove cost from nodes/edges 
            if (entity.ContainsProperty(
                nameof(ClientEntityMetadataTypes.TYPE_DETAILS.dense)
            ) && entity.GetProperty<bool>(
                nameof(ClientEntityMetadataTypes.TYPE_DETAILS.dense)
            ))
            {
                // If DensityBox remove cost from nodes/edges
                if (entity.ContainsProperty(
                    nameof(ClientEntityMetadataTypes.TYPE_DETAILS.densityBox)
                ))
                {
                    await _mediator.Send(
                        new RemoveEdgeCostForNodesAtPosition(
                            entity.Transform.Position,
                            entity.GetProperty<Vector3>(
                                nameof(ClientEntityMetadataTypes.TYPE_DETAILS.densityBox)
                            ),
                            500
                        )
                    );
                    return true;
                }
                else
                {
                    // Else remove it for just this postion
                    await _mediator.Send(
                        new RemoveEdgeCostForNodeAtPosition(
                            entity.Transform.Position,
                            500
                        )
                    );
                }
            }
            // Remove from Repository 
            _repository.Remove(
                request.GlobalId
            );
            return true;
        }
    }
}
