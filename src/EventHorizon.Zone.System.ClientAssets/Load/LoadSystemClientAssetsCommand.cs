namespace EventHorizon.Zone.System.ClientAssets.Load
{
    using EventHorizon.Zone.Core.Model.Command;
    using MediatR;

    public struct LoadSystemClientAssetsCommand
        : IRequest<StandardCommandResult>
    {
    }
}
