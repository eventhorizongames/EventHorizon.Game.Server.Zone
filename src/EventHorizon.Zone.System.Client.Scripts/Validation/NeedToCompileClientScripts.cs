namespace EventHorizon.Zone.System.Client.Scripts.Validation
{
    using EventHorizon.Zone.Core.Model.Command;
    using MediatR;

    public struct NeedToCompileClientScripts
        : IRequest<CommandResult<bool>>
    {
    }
}
