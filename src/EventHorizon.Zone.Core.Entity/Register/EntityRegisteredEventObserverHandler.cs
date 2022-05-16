namespace EventHorizon.Zone.Core.Events.Entity.Register;

using System.Threading;
using System.Threading.Tasks;

using EventHorizon.Observer.State;

using MediatR;

public class EntityRegisteredEventObserverHandler : INotificationHandler<EntityRegisteredEvent>
{
    private readonly ObserverState _observer;

    public EntityRegisteredEventObserverHandler(ObserverState observer)
    {
        _observer = observer;
    }

    public Task Handle(EntityRegisteredEvent notification, CancellationToken cancellationToken) =>
        _observer.Trigger<EntityRegisteredEventObserver, EntityRegisteredEvent>(
            notification,
            cancellationToken
        );
}
