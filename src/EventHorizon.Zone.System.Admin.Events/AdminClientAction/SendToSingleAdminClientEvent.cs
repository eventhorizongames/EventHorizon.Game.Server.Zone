namespace EventHorizon.Zone.System.Admin.AdminClientAction.Client;

using MediatR;

public struct SendToSingleAdminClientEvent
    : INotification
{
    public string ConnectionId { get; set; }
    public string Method { get; set; }
    public object Arg1 { get; set; }
    public object Arg2 { get; set; }
}
