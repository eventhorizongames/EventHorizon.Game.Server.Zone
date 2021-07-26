namespace EventHorizon.Zone.System.ClientAssets.Save
{
    using EventHorizon.Zone.Core.Model.Command;
    using MediatR;

    public class SaveClientAssetsCommand
        : IRequest<StandardCommandResult>
    {
    }
}