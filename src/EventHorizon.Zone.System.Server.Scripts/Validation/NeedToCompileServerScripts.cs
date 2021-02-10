namespace EventHorizon.Zone.System.Server.Scripts.Validation
{
    using EventHorizon.Zone.Core.Model.Command;
    using MediatR;

    public struct NeedToCompileServerScripts
        : IRequest<CommandResult<bool>>
    {
    }
}
