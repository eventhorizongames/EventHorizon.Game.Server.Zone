/// This should only be used Omnisharp to setup Scripting Environment.
/// The is used to intellisense the C# scripts.
using EventHorizon.Observer.Model;

using MediatR;

using Microsoft.Extensions.Logging;

public class ClientServices
{
    public static IMediator Mediator = null!;

    public static ILogger Logger<T>()
    {
        return default!;
    }

#pragma warning disable IDE0051 // Remove unused private members
    void RegisterObserver(
#pragma warning restore IDE0051 // Remove unused private members
#pragma warning disable IDE0060 // Remove unused parameter
        ObserverBase observer
#pragma warning restore IDE0060 // Remove unused parameter
    )
    { }

#pragma warning disable IDE0051 // Remove unused private members
    void UnRegisterObserver(
#pragma warning restore IDE0051 // Remove unused private members
#pragma warning disable IDE0060 // Remove unused parameter
        ObserverBase observer
#pragma warning restore IDE0060 // Remove unused parameter
    )
    { }
}


public static class ClientData
{
    public static T? Get<T>(
#pragma warning disable IDE0060 // Remove unused parameter
        string name
#pragma warning restore IDE0060 // Remove unused parameter
    ) => default;
}
