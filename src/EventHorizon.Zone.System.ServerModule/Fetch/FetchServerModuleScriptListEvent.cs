using System.Collections.Generic;
using EventHorizon.Zone.System.ServerModule.Model;
using MediatR;

namespace EventHorizon.Zone.System.ServerModule.Fetch
{
    public struct FetchServerModuleScriptListEvent : IRequest<IEnumerable<ServerModuleScripts>>
    {

    }
}