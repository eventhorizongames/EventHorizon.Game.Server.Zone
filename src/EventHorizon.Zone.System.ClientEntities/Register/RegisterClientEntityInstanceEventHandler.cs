using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Events.Map.Cost;
using EventHorizon.Zone.System.ClientEntities.State;
using MediatR;

namespace EventHorizon.Zone.System.ClientEntities.Register
{
    public struct RegisterClientEntityInstanceEventHandler : INotificationHandler<RegisterClientEntityInstanceEvent>
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
        public Task Handle(RegisterClientEntityInstanceEvent notification, CancellationToken cancellationToken)
        {
            _clientEntityRepository.Add(
                notification.ClientEntityInstance
            );
            // At postion if they are dense, increase cost to get to node
            if (notification.ClientEntityInstance.Properties != null
                && (bool)notification.ClientEntityInstance.Properties["dense"])
            {
                _mediator.Send(
                    new ChangeEdgeCostForNodeAtPositionCommand(
                        notification.ClientEntityInstance.Position,
                        500
                    )
                );
            }
            return Task.CompletedTask;
        }
    }
}