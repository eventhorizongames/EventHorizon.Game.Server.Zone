namespace EventHorizon.Zone.Core.Entity.Register;

using System.Threading;
using System.Threading.Tasks;

using EventHorizon.Zone.Core.Events.Entity.Register;
using EventHorizon.Zone.Core.Model.Entity.State;

using MediatR;

public class UnregisterEntityHandler
    : INotificationHandler<UnRegisterEntityEvent>
{
    private readonly IMediator _mediator;
    private readonly EntityRepository _entityRepository;

    public UnregisterEntityHandler(
        IMediator mediator,
        EntityRepository entityRepository
    )
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
