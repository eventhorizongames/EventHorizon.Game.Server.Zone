namespace EventHorizon.Zone.System.Server.Scripts.Events.Reload
{
    using EventHorizon.Zone.Core.Model.Command;
    using MediatR;

    public struct ReloadServerScriptsSystemCommand
        : IRequest<StandardCommandResult>
    {
    }
}
