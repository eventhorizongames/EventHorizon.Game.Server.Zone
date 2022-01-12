namespace EventHorizon.Zone.System.ClientAssets.Add;

using EventHorizon.Zone.System.ClientAssets.Api;

using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

public class AddClientAssetEventHandler
    : INotificationHandler<AddClientAssetEvent>
{
    private readonly ClientAssetRepository _assetRepository;

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
        _assetRepository.Add(notification.ClientAsset);

        return Task.CompletedTask;
    }
}
