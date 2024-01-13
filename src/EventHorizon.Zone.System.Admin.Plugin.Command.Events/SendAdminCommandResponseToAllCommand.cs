namespace EventHorizon.Zone.System.Admin.Plugin.Command.Events;

using EventHorizon.Zone.System.Admin.Plugin.Command.Model;

using MediatR;

public struct SendAdminCommandResponseToAllCommand
     : IRequest<bool>
{
    public IAdminCommandResponse Response { get; }

    public SendAdminCommandResponseToAllCommand(
        IAdminCommandResponse response
    )
    {
        Response = response;
    }
}
