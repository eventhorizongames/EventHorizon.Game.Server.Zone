namespace EventHorizon.Zone.System.Server.Scripts.Complie;

using EventHorizon.Zone.Core.Model.Command;

using MediatR;

public struct CompileServerScriptsFromSubProcessCommand
    : IRequest<StandardCommandResult>
{
}
