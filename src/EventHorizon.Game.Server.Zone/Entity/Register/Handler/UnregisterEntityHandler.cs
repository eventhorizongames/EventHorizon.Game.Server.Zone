using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Events.Entity.Register;
using EventHorizon.Zone.Core.Model.Entity;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Entity.Register.Handler
{
    public class UnregisterEntityHandler : INotificationHandler<UnRegisterEntityEvent>
    {
        readonly IMediator _mediator;
        readonly IEntityRepository _entityRepository;
        public UnregisterEntityHandler(IMediator mediator, IEntityRepository entityRepository)
        {
            _mediator = mediator;
            _entityRepository = entityRepository;
        }

        public async Task Handle(
            UnRegisterEntityEvent notification,
            CancellationToken cancellationToken
        )
        {
            await _entityRepository.Remove(
                notification.Entity.Id
            );
            await _mediator.Publish(
                new EntityUnRegisteredEvent
                {
                    EntityId = notification.Entity.Id,
                }
            );
        }
    }
}