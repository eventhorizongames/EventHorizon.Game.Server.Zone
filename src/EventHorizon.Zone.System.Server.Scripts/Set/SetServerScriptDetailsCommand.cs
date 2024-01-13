namespace EventHorizon.Zone.System.Server.Scripts.Set;

using EventHorizon.Zone.Core.Model.Command;
using EventHorizon.Zone.System.Server.Scripts.Model.Details;

using MediatR;

public struct SetServerScriptDetailsCommand
    : IRequest<StandardCommandResult>
{
    public ServerScriptDetails ScriptDetails { get; }

    public SetServerScriptDetailsCommand(
        ServerScriptDetails scriptDetails
    )
    {
        ScriptDetails = scriptDetails;
    }
}
