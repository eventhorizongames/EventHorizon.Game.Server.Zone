using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Events.Map.Cost;
using EventHorizon.Zone.System.ClientEntities.State;
using EventHorizon.Zone.System.ClientEntities.Api;
using MediatR;

namespace EventHorizon.Zone.System.ClientEntities.Register
{
    public class RegisterClientEntityInstanceEventHandler : INotificationHandler<RegisterClientEntityInstanceEvent>
    {
        readonly IMediator _mediator;
        readonly ClientEntityInstanceRepository _clientEntityRepository;
        public RegisterClientEntityInstanceEventHandler(
            IMediator mediator,
            ClientEntityInstanceRepository entityRepository
        )
        {
            _mediator = mediator;
            _clientEntityRepository = entityRepository;
        }
        public async Task Handle(RegisterClientEntityInstanceEvent notification, CancellationToken cancellationToken)
        {
            var entity = notification.ClientEntityInstance;
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
                            entity.Position,
                            entity.GetProperty<Vector3>("densityBox"),
                            500
                        )
                    );
                    return;
                }
                await _mediator.Send(
                    new ChangeEdgeCostForNodeAtPositionCommand(
                        entity.Position,
                        500
                    )
                );
            }
        }

        private static bool ContainProperty(
            Model.ClientEntityInstance entity,
            string property
        )
        {
            return entity.Properties?.ContainsKey(
                property
            ) ?? false;
        }
    }
}