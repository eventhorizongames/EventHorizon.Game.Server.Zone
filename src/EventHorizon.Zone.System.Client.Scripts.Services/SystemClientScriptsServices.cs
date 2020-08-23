/// This should only be used Omnisharp to setup Scripting Environment.
/// The is used to intellisense the C# scripts.
using MediatR;

public class ClientServices
{
    public static IMediator Mediator;
}


public static class ClientData
{
    public static T Get<T>(
        string name
    ) => default(T);
}