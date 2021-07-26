namespace EventHorizon.Zone.System.ClientAssets.Fetch
{
    using EventHorizon.Zone.System.ClientAssets.Model;
    using EventHorizon.Zone.System.ClientAssets.State.Api;
    using global::System.Collections.Generic;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using MediatR;

    public class FetchClientAssetListQueryHandler 
        : IRequestHandler<FetchClientAssetListQuery, IEnumerable<ClientAsset>>
    {
        readonly ClientAssetRepository _assetRepository;
        public FetchClientAssetListQueryHandler(
            ClientAssetRepository assetRepository
        )
        {
            _assetRepository = assetRepository;
        }
        public Task<IEnumerable<ClientAsset>> Handle(
            FetchClientAssetListQuery request,
            CancellationToken cancellationToken
        )
        {
            return _assetRepository
                .All()
                .FromResult();
        }
    }
}