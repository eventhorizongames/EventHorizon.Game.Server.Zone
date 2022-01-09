namespace EventHorizon.Zone.System.EntityModule.Fetch;

using EventHorizon.Zone.System.EntityModule.Model;

using global::System.Collections.Generic;

using MediatR;

public struct FetchPlayerModuleListQuery
    : IRequest<IEnumerable<EntityScriptModule>>
{

}
