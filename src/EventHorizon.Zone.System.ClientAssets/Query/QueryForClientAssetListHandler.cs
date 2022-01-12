namespace EventHorizon.Zone.System.ClientAssets.Query;

using EventHorizon.Zone.System.ClientAssets.Api;
using EventHorizon.Zone.System.ClientAssets.Model;

using global::System.Collections.Generic;
using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

public class QueryForClientAssetListHandler
    : IRequestHandler<
          QueryForClientAssetList,
          IEnumerable<ClientAsset>
      >
{
    private readonly ClientAssetRepository _assetRepository;

    public QueryForClientAssetListHandler(
        ClientAssetRepository assetRepository
    )
    {
        _assetRepository = assetRepository;
    }

    public Task<IEnumerable<ClientAsset>> Handle(
        QueryForClientAssetList request,
        CancellationToken cancellationToken
    )
    {
        return _assetRepository.All().FromResult();
    }
}
