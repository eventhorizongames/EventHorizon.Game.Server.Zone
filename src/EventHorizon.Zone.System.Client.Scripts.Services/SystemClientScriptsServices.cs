/// This should only be used Omnisharp to setup Scripting Environment.
/// The is used to intellisense the C# scripts.
using EventHorizon.Observer.Model;

using MediatR;

using Microsoft.Extensions.Logging;

#pragma warning disable CA1050 // Declare types in namespaces
public class ClientServices
#pragma warning restore CA1050 // Declare types in namespaces
{
    public static IMediator Mediator = null!;

    public static ILogger Logger<T>()
    {
        return default!;
    }

#pragma warning disable IDE0051 // Remove unused private members
#pragma warning disable CA1822 // Mark members as static
    void RegisterObserver(
#pragma warning restore CA1822 // Mark members as static
#pragma warning restore IDE0051 // Remove unused private members
#pragma warning disable IDE0060 // Remove unused parameter
        ObserverBase observer
#pragma warning restore IDE0060 // Remove unused parameter
    )
    { }

#pragma warning disable IDE0051 // Remove unused private members
#pragma warning disable CA1822 // Mark members as static
    void UnRegisterObserver(
#pragma warning restore CA1822 // Mark members as static
#pragma warning restore IDE0051 // Remove unused private members
#pragma warning disable IDE0060 // Remove unused parameter
        ObserverBase observer
#pragma warning restore IDE0060 // Remove unused parameter
    )
    { }
}


#pragma warning disable CA1050 // Declare types in namespaces
public static class ClientData
#pragma warning restore CA1050 // Declare types in namespaces
{
    public static T? Get<T>(
#pragma warning disable IDE0060 // Remove unused parameter
        string name
#pragma warning restore IDE0060 // Remove unused parameter
    ) => default;
}
