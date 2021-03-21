namespace EventHorizon.Zone.System.Server.Scripts.Load
{
    using EventHorizon.Zone.Core.Model.Command;
    using MediatR;

    public struct LoadNewServerScriptAssemblyCommand
        : IRequest<StandardCommandResult>
    {
    }
}
