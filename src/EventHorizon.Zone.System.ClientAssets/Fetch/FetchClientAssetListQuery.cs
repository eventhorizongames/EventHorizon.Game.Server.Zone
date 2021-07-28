namespace EventHorizon.Zone.System.ClientAssets.Fetch
{
    using EventHorizon.Zone.System.ClientAssets.Model;
    using global::System.Collections.Generic;
    using MediatR;

    public struct FetchClientAssetListQuery
        : IRequest<IEnumerable<ClientAsset>>
    {

    }
}
