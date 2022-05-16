namespace EventHorizon.Zone.System.Server.Scripts.System;

using EventHorizon.Observer.Model;
using EventHorizon.Observer.State;
using EventHorizon.Zone.System.Server.Scripts.Model;

using global::System.Threading;
using global::System.Threading.Tasks;

public class SystemServerScriptObserverBroker : ServerScriptObserverBroker
{
    private readonly ObserverState _state;

    public SystemServerScriptObserverBroker(ObserverState state)
    {
        _state = state;
    }

    public async Task Trigger<TInstance, TArgs>(
        TArgs args,
        CancellationToken cancellationToken = default
    ) where TInstance : ArgumentObserver<TArgs>
    {
        await _state.Trigger<TInstance, TArgs>(args, cancellationToken);
    }

    public async Task Trigger<TEvent, TObserver>(
        ObserverableMessageBase<TEvent, TObserver> message,
        CancellationToken cancellationToken = default
    )
        where TObserver : ArgumentObserver<TEvent>
        where TEvent : ObserverableMessageBase<TEvent, TObserver>
    {
        await message.Send(_state, cancellationToken);
    }
}
