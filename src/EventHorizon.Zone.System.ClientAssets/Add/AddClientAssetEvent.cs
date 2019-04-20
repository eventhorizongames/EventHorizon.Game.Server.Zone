using EventHorizon.Zone.System.ClientEntity.Api;
using MediatR;

namespace EventHorizon.Zone.System.ClientAssets.Add
{
    public struct AddClientAssetEvent : INotification
    {
        public IClientAsset ClientAsset { get; internal set; }
    }
}