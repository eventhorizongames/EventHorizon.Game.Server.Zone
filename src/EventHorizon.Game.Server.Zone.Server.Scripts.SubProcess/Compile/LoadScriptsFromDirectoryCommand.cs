namespace EventHorizon.Game.Server.Zone.Server.Scripts.SubProcess.Compile;

using System;
using System.Collections;
using System.Collections.Generic;

using EventHorizon.Zone.Core.Model.Command;
using EventHorizon.Zone.System.Server.Scripts.Model.Details;

using MediatR;

public struct LoadScriptsFromDirectoryCommand
    : IRequest<CommandResult<IEnumerable<ServerScriptDetails>>>
{
    public string DirectoryFullName { get; }
    public IDictionary<string, object> Arguments { get; }

    public LoadScriptsFromDirectoryCommand(
        string directoryFullName,
        IDictionary<string, object> arguments
    )
    {
        DirectoryFullName = directoryFullName;
        Arguments = arguments;
    }
}
