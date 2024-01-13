namespace EventHorizon.Zone.System.Admin.Plugin.Command.Events;

using EventHorizon.Zone.System.Admin.Plugin.Command.Model;

using MediatR;

public struct RespondToAdminCommand
    : IRequest<bool>
{
    public string? ConnectionId { get; }
    public IAdminCommandResponse Response { get; }

    public RespondToAdminCommand(
        string? connectionId,
        IAdminCommandResponse response
    )
    {
        ConnectionId = connectionId;
        Response = response;
    }
}
