namespace EventHorizon.Zone.System.ClientAssets.Query;

using EventHorizon.Zone.System.ClientAssets.Model;

using global::System.Collections.Generic;

using MediatR;

public struct QueryForClientAssetList
    : IRequest<IEnumerable<ClientAsset>> { }
