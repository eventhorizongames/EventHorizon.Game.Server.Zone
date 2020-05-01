namespace EventHorizon.Zone.System.ClientEntities.Register
{
    using EventHorizon.Zone.Core.Events.Map.Cost;
    using EventHorizon.Zone.Core.Model.Entity;
    using EventHorizon.Zone.System.ClientEntities.Model;
    using EventHorizon.Zone.System.ClientEntities.PopulateData;
    using EventHorizon.Zone.System.ClientEntities.State;
    using global::System.Numerics;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using MediatR;

    public class RegisterClientEntityCommandHandler : IRequestHandler<RegisterClientEntityCommand>
    {
        private readonly IMediator _mediator;
        private readonly ClientEntityRepository _clientEntityRepository;

        public RegisterClientEntityCommandHandler(
            IMediator mediator,
            ClientEntityRepository entityRepository
        )
        {
            _mediator = mediator;
            _clientEntityRepository = entityRepository;
        }

        public async Task<Unit> Handle(
            RegisterClientEntityCommand request,
            CancellationToken cancellationToken
        )
        {
            var entity = request.ClientEntity;
            await _mediator.Publish(
                new PopulateClientEntityDataEvent(
                    entity
                )
            );
            _clientEntityRepository.Add(
                entity
            );
            // At postion if they are dense, increase cost to get to node
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
                        )
                    );
                    return Unit.Value;
                }
                else
                {
                    await _mediator.Send(
                        new ChangeEdgeCostForNodeAtPosition(
                            entity.Transform.Position,
                            500
                        )
                    );
                }
            }
            return Unit.Value;
        }
    }
}