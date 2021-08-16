/// This should only be used Omnisharp to setup Scripting Environment.
/// The is used to intellisense the C# scripts.
using EventHorizon.Observer.Model;

using MediatR;

using Microsoft.Extensions.Logging;

public class ClientServices
{
    public static IMediator Mediator;

    public static ILogger Logger<T>()
    {
        return default;
    }

    void RegisterObserver(
        ObserverBase observer
    )
    { }

    void UnRegisterObserver(
        ObserverBase observer
    )
    { }
}


public static class ClientData
{
    public static T Get<T>(
        string name
    ) => default(T);
}
