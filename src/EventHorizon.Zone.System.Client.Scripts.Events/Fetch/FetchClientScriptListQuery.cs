namespace EventHorizon.Zone.System.Client.Scripts.Events.Fetch;

using EventHorizon.Zone.System.Client.Scripts.Model;

using global::System.Collections.Generic;

using MediatR;

public struct FetchClientScriptListQuery
    : IRequest<IEnumerable<ClientScript>>
{

}
