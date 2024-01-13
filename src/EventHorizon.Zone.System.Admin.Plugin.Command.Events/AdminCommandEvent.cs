namespace EventHorizon.Zone.System.Admin.Plugin.Command.Events;

using EventHorizon.Zone.System.Admin.Plugin.Command.Model;

using MediatR;

public struct AdminCommandEvent : INotification
{
    public string? ConnectionId { get; }
    public IAdminCommand Command { get; }
    public object Data { get; }

    public AdminCommandEvent(
        string connectionId,
        IAdminCommand command,
        object data
    )
    {
        ConnectionId = connectionId;
        Command = command;
        Data = data;
    }

    public AdminCommandEvent(
        IAdminCommand command,
        object data
    )
    {
        ConnectionId = null;
        Command = command;
        Data = data;
    }
}
