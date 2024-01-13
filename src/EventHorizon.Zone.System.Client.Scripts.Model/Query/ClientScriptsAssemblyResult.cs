namespace EventHorizon.Zone.System.Client.Scripts.Model.Query;

using System;

public struct ClientScriptsAssemblyResult
{
    public string Hash { get; }
    public string ScriptAssembly { get; }

    public ClientScriptsAssemblyResult(
        string hash,
        string scriptAssembly
    )
    {
        Hash = hash;
        ScriptAssembly = scriptAssembly;
    }
}
