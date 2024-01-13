namespace EventHorizon.Zone.System.Server.Scripts.Plugin.Compiler.Assemblies;

using global::System.Collections.Generic;
using global::System.Reflection;

using MediatR;

public struct QueryForScriptAssemblyList
    : IRequest<IEnumerable<Assembly>>
{
}
