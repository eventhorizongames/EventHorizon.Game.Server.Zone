namespace EventHorizon.Zone.System.Server.Scripts.Model;

using EventHorizon.Observer.Model;
using EventHorizon.Observer.State;

using global::System.Threading;
using global::System.Threading.Tasks;

public abstract class ObserverableMessageBase<TEvent, TObserver>
    where TObserver : ArgumentObserver<TEvent>
    where TEvent : ObserverableMessageBase<TEvent, TObserver>
{
    public async Task Send(ObserverState state, CancellationToken cancellationToken) =>
        await state.Trigger<TObserver, TEvent>((TEvent)this, cancellationToken);
}
