using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.System.ClientEntities.State;
using MediatR;

namespace EventHorizon.Zone.System.ClientEntities.Register
{
    public struct RegisterClientEntityInstanceEventHandler : INotificationHandler<RegisterClientEntityInstanceEvent>
    {
        readonly ClientEntityInstanceRepository _clientEntityRepository;
        public RegisterClientEntityInstanceEventHandler(
            ClientEntityInstanceRepository entityRepository
        )
        {
            _clientEntityRepository = entityRepository;
        }
        public Task Handle(RegisterClientEntityInstanceEvent notification, CancellationToken cancellationToken)
        {
            _clientEntityRepository.Add(
                notification.ClientEntityInstance
            );
            return Task.CompletedTask;
        }
    }
}