namespace EventHorizon.Zone.System.ClientAssets.Events.Update
{
    using EventHorizon.Zone.Core.Model.Command;
    using EventHorizon.Zone.System.ClientAssets.Model;
    using MediatR;

    public struct UpdateClientAssetCommand
        : IRequest<StandardCommandResult>
    {
        public ClientAsset ClientAsset { get; }

        public UpdateClientAssetCommand(
            ClientAsset clientAsset
        )
        {
            ClientAsset = clientAsset;
        }
    }
}
