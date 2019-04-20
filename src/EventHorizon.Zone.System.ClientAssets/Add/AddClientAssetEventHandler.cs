using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.System.ClientAssets.State;
using MediatR;

namespace EventHorizon.Zone.System.ClientAssets.Add
{
    public struct AddClientAssetEventHandler : INotificationHandler<AddClientAssetEvent>
    {
        readonly ClientAssetRepository _assetRepository;
        public AddClientAssetEventHandler(
            ClientAssetRepository assetRepository
        )
        {
            _assetRepository = assetRepository;
        }
        public Task Handle(
            AddClientAssetEvent notification,
            CancellationToken cancellationToken
        )
        {
            _assetRepository.Add(
                notification.ClientAsset
            );
            return Task.CompletedTask;
        }
    }
}