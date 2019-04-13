using System.Collections.Generic;
using EventHorizon.Zone.System.EntityModule.Model;
using MediatR;

namespace EventHorizon.Zone.System.EntityModule.Fetch
{
    public struct FetchPlayerModuleListQuery : IRequest<IEnumerable<EntityScriptModule>>
    {

    }
}