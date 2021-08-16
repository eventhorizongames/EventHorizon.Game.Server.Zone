namespace EventHorizon.Zone.System.ServerModule.Fetch
{
    using EventHorizon.Zone.System.ServerModule.Model;

    using global::System.Collections.Generic;

    using MediatR;

    public struct FetchServerModuleScriptList : IRequest<IEnumerable<ServerModuleScripts>>
    {

    }
}
