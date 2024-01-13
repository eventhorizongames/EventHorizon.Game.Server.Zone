namespace EventHorizon.Zone.System.Client.Scripts.Plugin.Compiler.Assemblies;

using EventHorizon.Zone.Core.Model.Info;

using global::System.Collections.Generic;
using global::System.IO;
using global::System.Linq;
using global::System.Reflection;
using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;


public class QueryForScriptAssemblyListHandler
    : IRequestHandler<QueryForScriptAssemblyList, IEnumerable<Assembly>>
{
    private readonly ServerInfo _serverInfo;

    public QueryForScriptAssemblyListHandler(
        ServerInfo serverInfo
    )
    {
        _serverInfo = serverInfo;
    }

    public Task<IEnumerable<Assembly>> Handle(
        QueryForScriptAssemblyList request,
        CancellationToken cancellationToken
    ) => Directory.GetFiles(
        _serverInfo.AssembliesPath,
        "EventHorizon.*.dll"
    ).Select(
        x => Assembly.Load(
            AssemblyName.GetAssemblyName(x)
        )
    ).FromResult();
}
