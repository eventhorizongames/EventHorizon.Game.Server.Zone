namespace EventHorizon.Zone.System.EntityModule.Fetch
{
    using global::System.Collections.Generic;

    using EventHorizon.Zone.System.EntityModule.Model;

    using MediatR;

    public struct FetchPlayerModuleListQuery : IRequest<IEnumerable<EntityScriptModule>>
    {

    }
}
