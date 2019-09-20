using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Entity.Model;
using EventHorizon.Game.Server.Zone.Entity.Registered;
using EventHorizon.Game.Server.Zone.Entity.State;
using EventHorizon.Zone.Core.Model.Entity;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Entity.Register.Handler
{
    public class UnregisterEntityHandler : INotificationHandler<UnregisterEntityEvent>
    {
        readonly IMediator _mediator;
        readonly IEntityRepository _entityRepository;
        public UnregisterEntityHandler(IMediator mediator, IEntityRepository entityRepository)
        {
            _mediator = mediator;
            _entityRepository = entityRepository;
        }

        public async Task Handle(UnregisterEntityEvent notification, CancellationToken cancellationToken)
        {
            await _entityRepository.Remove(notification.Entity.Id);
            await _mediator.Publish(new EntityUnregisteredEvent
            {
                EntityId = notification.Entity.Id,
            });
        }
    }
}