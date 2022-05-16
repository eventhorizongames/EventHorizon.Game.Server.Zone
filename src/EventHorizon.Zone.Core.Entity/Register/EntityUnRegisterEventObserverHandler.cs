namespace EventHorizon.Zone.Core.Events.Entity.Register;

using System.Threading;
using System.Threading.Tasks;

using EventHorizon.Observer.State;

using MediatR;

public class EntityUnRegisterEventObserverHandler : INotificationHandler<EntityUnRegisteredEvent>
{
    private readonly ObserverState _observer;

    public EntityUnRegisterEventObserverHandler(ObserverState observer)
    {
        _observer = observer;
    }

    public Task Handle(EntityUnRegisteredEvent notification, CancellationToken cancellationToken) =>
        _observer.Trigger<EntityUnRegisterEventObserver, EntityUnRegisteredEvent>(
            notification,
            cancellationToken
        );
}
