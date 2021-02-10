namespace EventHorizon.Zone.System.Server.Scripts.Plugin.Compiler.Model
{
    public struct CompiledScriptResult
    {
        public bool Success { get; }
        public string ErrorCode { get; }

        public string Hash { get; }

        public CompiledScriptResult(
            string hash
        )
        {
            Success = true;
            ErrorCode = string.Empty;
            Hash = hash;
        }

        public CompiledScriptResult(
            bool success,
            string errorCode
        )
        {
            Success = success;
            ErrorCode = errorCode;
            Hash = string.Empty;
        }
    }
}
