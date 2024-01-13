namespace EventHorizon.Zone.System.ClientEntities.Map;

using EventHorizon.Zone.Core.Events.Map.Create;
using EventHorizon.Zone.System.ClientEntities.State;
using EventHorizon.Zone.System.ClientEntities.Update;

using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

public class MapCreatedForClientEntitiesHandler : INotificationHandler<MapCreatedEvent>
{
    private readonly IMediator _mediator;
    private readonly ClientEntityRepository _repository;

    public MapCreatedForClientEntitiesHandler(
        IMediator mediator,
        ClientEntityRepository repository
    )
    {
        _mediator = mediator;
        _repository = repository;
    }

    public async Task Handle(
        MapCreatedEvent notification,
        CancellationToken cancellationToken
    )
    {
        foreach (var entity in _repository.All())
        {
            await _mediator.Send(
                new SetClientEntityNodeDensity(
                    entity
                )
            );
        }
    }
}
