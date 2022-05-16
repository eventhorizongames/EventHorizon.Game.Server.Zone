namespace EventHorizon.Zone.System.Server.Scripts.Model;

using EventHorizon.Observer.Model;

using global::System.Threading;
using global::System.Threading.Tasks;

public interface ServerScriptObserverBroker
{
    Task Trigger<TInstance, TArgs>(TArgs args, CancellationToken cancellationToken = default)
        where TInstance : ArgumentObserver<TArgs>;

    Task Trigger<TEvent, TObserver>(
        ObserverableMessageBase<TEvent, TObserver> message,
        CancellationToken cancellationToken = default
    )
        where TObserver : ArgumentObserver<TEvent>
        where TEvent : ObserverableMessageBase<TEvent, TObserver>;
}
