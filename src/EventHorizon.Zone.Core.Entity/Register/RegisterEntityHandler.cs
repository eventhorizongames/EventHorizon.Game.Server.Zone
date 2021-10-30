namespace EventHorizon.Zone.Core.Entity.Register
{
    using System.Threading;
    using System.Threading.Tasks;

    using EventHorizon.Zone.Core.Events.Entity.Data;
    using EventHorizon.Zone.Core.Events.Entity.Register;
    using EventHorizon.Zone.Core.Model.Entity;
    using EventHorizon.Zone.Core.Model.Entity.State;
    using EventHorizon.Zone.Core.Set;

    using MediatR;

    using Microsoft.Extensions.Logging;

    public class RegisterEntityHandler
        : IRequestHandler<RegisterEntityEvent, IObjectEntity>
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly EntityRepository _entityRepository;

        public RegisterEntityHandler(
            ILogger<RegisterEntityHandler> logger,
            IMediator mediator,
            EntityRepository entityRepository
        )
        {
            _logger = logger;
            _mediator = mediator;
            _entityRepository = entityRepository;
        }

        public async Task<IObjectEntity> Handle(
            RegisterEntityEvent notification,
            CancellationToken cancellationToken
        )
        {
            var entity = await _entityRepository.Add(
                notification.Entity
            );

            var result = await _mediator.Send(
                new SetEntityPropertyOverrideDataCommand(
                    notification.Entity
                ),
                cancellationToken
            );
            if (!result)
            {
                _logger.LogWarning(
                    "Failed to override Entity Property Data: {ErrorCode}",
                    result.ErrorCode
                );
            }

            await _mediator.Publish(
                new PrePopulateEntityDataEvent(
                    entity
                ),
                cancellationToken
            );

            await _mediator.Publish(
                new PopulateEntityDataEvent(
                    entity
                ),
                cancellationToken
            );

            await _mediator.Publish(
                new EntityRegisteredEvent
                {
                    Entity = entity,
                },
                cancellationToken
            );

            return entity;
        }
    }
}
