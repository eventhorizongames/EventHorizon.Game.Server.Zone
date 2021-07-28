namespace EventHorizon.Zone.System.ClientAssets.Save
{
    using EventHorizon.Zone.Core.Model.Command;
    using MediatR;

    public struct SaveClientAssetsCommand
        : IRequest<StandardCommandResult>
    {
    }
}
