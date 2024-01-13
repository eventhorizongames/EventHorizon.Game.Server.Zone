namespace EventHorizon.Zone.Core.Events.Client;

using MediatR;

public struct SendToAllClientsEvent : INotification
{
    public string Method { get; set; }
    public object Arg1 { get; set; }
    public object Arg2 { get; set; }
}
