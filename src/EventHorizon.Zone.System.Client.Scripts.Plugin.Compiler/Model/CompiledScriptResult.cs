namespace EventHorizon.Zone.System.Client.Scripts.Plugin.Compiler.Model;

public struct CompiledScriptResult
{
    public bool Success { get; }
    public string ErrorCode { get; }

    public string Hash { get; }
    public string ScriptAssembly { get; }

    public CompiledScriptResult(
        string hash,
        string scriptAssembly
    )
    {
        Success = true;
        ErrorCode = string.Empty;
        Hash = hash;
        ScriptAssembly = scriptAssembly;
    }

    public CompiledScriptResult(
        bool success,
        string errorCode
    )
    {
        Success = success;
        ErrorCode = errorCode;
        Hash = string.Empty;
        ScriptAssembly = string.Empty;
    }
}
