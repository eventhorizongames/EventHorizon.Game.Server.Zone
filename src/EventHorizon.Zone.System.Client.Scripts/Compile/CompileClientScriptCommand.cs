namespace EventHorizon.Zone.System.Client.Scripts.Compile
{
    using EventHorizon.Zone.Core.Model.Command;
    using MediatR;

    public struct CompileClientScriptCommand
        : IRequest<StandardCommandResult>
    {
    }
}
