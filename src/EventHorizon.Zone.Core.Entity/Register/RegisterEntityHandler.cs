using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Events.Entity.Data;
using EventHorizon.Zone.Core.Events.Entity.Register;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.Core.Model.Entity.State;
using MediatR;

namespace EventHorizon.Zone.Core.Entity.Register
{
    public class RegisterEntityHandler : IRequestHandler<RegisterEntityEvent, IObjectEntity>
    {
        readonly IMediator _mediator;
        readonly EntityRepository _entityRepository;
        
        public RegisterEntityHandler(
            IMediator mediator,
            EntityRepository entityRepository
        )
        {
            _mediator = mediator;
            _entityRepository = entityRepository;
        }

        public async Task<IObjectEntity> Handle(
            RegisterEntityEvent notification,
            CancellationToken cancellationToken
        )
        {
            // TODO: Add a validation of Registered Entity
            var entity = await _entityRepository.Add(
                notification.Entity
            );
            await _mediator.Publish(
                new PopulateEntityDataEvent
                {
                    Entity = entity,
                }
            );
            await _mediator.Publish(
                new EntityRegisteredEvent
                {
                    Entity = entity,
                }
            );
            return entity;
        }
    }
}