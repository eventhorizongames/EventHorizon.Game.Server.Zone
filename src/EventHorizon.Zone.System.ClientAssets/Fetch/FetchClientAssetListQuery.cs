using System.Collections.Generic;
using EventHorizon.Zone.System.ClientEntity.Api;
using MediatR;

namespace EventHorizon.Zone.System.ClientAssets.Fetch
{
    public struct FetchClientAssetListQuery : IRequest<IEnumerable<IClientAsset>>
    {

    }
}