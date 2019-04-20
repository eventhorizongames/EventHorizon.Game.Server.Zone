using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.System.ClientAssets.State;
using EventHorizon.Zone.System.ClientEntity.Api;
using MediatR;

namespace EventHorizon.Zone.System.ClientAssets.Fetch
{
    public struct FetchClientAssetListQueryHandler : IRequestHandler<FetchClientAssetListQuery, IEnumerable<IClientAsset>>
    {
        readonly ClientAssetRepository _assetRepository;
        public FetchClientAssetListQueryHandler(
            ClientAssetRepository assetRepository
        )
        {
            _assetRepository = assetRepository;
        }
        public Task<IEnumerable<IClientAsset>> Handle(
            FetchClientAssetListQuery request,
            CancellationToken cancellationToken
        )
        {
            return Task.FromResult(
                _assetRepository.All()
            );
        }
    }
}