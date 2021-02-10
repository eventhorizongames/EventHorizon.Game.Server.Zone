namespace EventHorizon.Game.Server.Zone.Server.Scripts.SubProcess.Compile
{
    using System;
    using EventHorizon.Zone.Core.Model.Command;
    using MediatR;

    public struct CompileServerScriptsCommand
        : IRequest<StandardCommandResult>
    {
    }
}
