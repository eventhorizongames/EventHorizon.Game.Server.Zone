namespace EventHorizon.Game.Server.Zone.Client.Scripts.SubProcess.Compile;

using System;

using EventHorizon.Zone.Core.Model.Command;

using MediatR;

public struct CompileClientScriptsCommand
    : IRequest<StandardCommandResult>
{
}
