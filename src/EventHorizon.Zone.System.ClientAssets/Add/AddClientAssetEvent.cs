namespace EventHorizon.Zone.System.ClientAssets.Add;

using EventHorizon.Zone.System.ClientAssets.Model;

using MediatR;

public struct AddClientAssetEvent : INotification
{
    public ClientAsset ClientAsset { get; }

    public AddClientAssetEvent(ClientAsset clientAsset)
    {
        ClientAsset = clientAsset;
    }
}
