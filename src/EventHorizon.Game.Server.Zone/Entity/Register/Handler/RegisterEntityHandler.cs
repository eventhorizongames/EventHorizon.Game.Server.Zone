using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Entity.Model;
using EventHorizon.Game.Server.Zone.Entity.Registered;
using EventHorizon.Game.Server.Zone.Entity.State;
using EventHorizon.Game.Server.Zone.External.Entity;
using EventHorizon.Game.Server.Zone.Model.Entity;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Entity.Register.Handler
{
    public class RegisterEntityHandler : IRequestHandler<RegisterEntityEvent, IObjectEntity>
    {
        readonly IMediator _mediator;
        readonly IEntityRepository _entityRepository;
        public RegisterEntityHandler(IMediator mediator, IEntityRepository entityRepository)
        {
            _mediator = mediator;
            _entityRepository = entityRepository;
        }

        public async Task<IObjectEntity> Handle(RegisterEntityEvent notification, CancellationToken cancellationToken)
        {
            // TODO: Add a validation of Registered Entity
            var entity = await _entityRepository.Add(notification.Entity);
            await _mediator.Publish(new EntityRegisteredEvent
            {
                Entity = entity,
            });
            return entity;
        }
    }
}