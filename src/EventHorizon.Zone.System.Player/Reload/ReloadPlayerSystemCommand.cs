namespace EventHorizon.Zone.System.Player.Reload
{
    using EventHorizon.Zone.Core.Model.Command;

    using MediatR;

    public struct ReloadPlayerSystemCommand
        : IRequest<StandardCommandResult>
    {
    }
}
