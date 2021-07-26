namespace EventHorizon.Zone.System.ClientAssets.Events.Create
{
    using EventHorizon.Zone.Core.Model.Command;
    using EventHorizon.Zone.System.ClientAssets.Model;
    using MediatR;

    public struct CreateClientAssetCommand
        : IRequest<StandardCommandResult>
    {
        public ClientAsset ClientAsset { get; }

        public CreateClientAssetCommand(
            ClientAsset clientAsset
        )
        {
            ClientAsset = clientAsset;
        }
    }
}
