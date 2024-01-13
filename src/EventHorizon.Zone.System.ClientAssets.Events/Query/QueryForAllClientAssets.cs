namespace EventHorizon.Zone.System.ClientAssets.Events.Query;

using EventHorizon.Zone.Core.Model.Command;
using EventHorizon.Zone.System.ClientAssets.Model;

using global::System.Collections.Generic;

using MediatR;

public struct QueryForAllClientAssets
    : IRequest<CommandResult<IEnumerable<ClientAsset>>>
{
}
