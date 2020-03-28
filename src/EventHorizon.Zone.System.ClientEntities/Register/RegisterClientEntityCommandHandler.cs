namespace EventHorizon.Zone.System.ClientEntities.Register
{
    using global::System.Numerics;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using EventHorizon.Zone.Core.Events.Map.Cost;
    using MediatR;
    using EventHorizon.Zone.System.ClientEntities.State;
    using EventHorizon.Zone.System.ClientEntities.Model;
    using EventHorizon.Zone.Core.Model.Entity;
    using EventHorizon.Zone.System.ClientEntities.PopulateData;

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
            if (ContainProperty(
                entity,
                "dense"
            ))
            {
                if (ContainProperty(
                    entity,
                    "densityBox"
                ))
                {
                    await _mediator.Send(
                        new ChangeEdgeCostForNodesAtPositionCommand(
                            entity.Transform.Position,
                            entity.GetProperty<Vector3>("densityBox"),
                            500
                        )
                    );
                    return Unit.Value;
                }
                await _mediator.Send(
                    new ChangeEdgeCostForNodeAtPositionCommand(
                        entity.Transform.Position,
                        500
                    )
                );
            }
            return Unit.Value;
        }

        private static bool ContainProperty(
            ClientEntity entity,
            string property
        )
        {
            return entity.Data?.ContainsKey(
                property
            ) ?? false;
        }
    }
}