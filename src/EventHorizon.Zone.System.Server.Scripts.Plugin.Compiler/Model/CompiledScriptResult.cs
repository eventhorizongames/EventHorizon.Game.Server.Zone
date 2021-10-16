namespace EventHorizon.Zone.System.Server.Scripts.Plugin.Compiler.Model
{
    using EventHorizon.Zone.System.Server.Scripts.Plugin.Shared.Model;

    using global::System.Collections.Generic;

    public struct CompiledScriptResult
    {
        public bool Success { get; }
        public string ErrorCode { get; }
        public List<GeneratedServerScriptErrorDetailsModel>? ScriptErrorDetailsList { get; set; }


        public string Hash { get; }

        public CompiledScriptResult(
            string hash
        )
        {
            Success = true;
            ErrorCode = string.Empty;
            ScriptErrorDetailsList = null;
            Hash = hash;
        }

        public CompiledScriptResult(
            bool success,
            string errorCode,
            List<GeneratedServerScriptErrorDetailsModel> scriptErrorDetailsList
        )
        {
            Success = success;
            ErrorCode = errorCode;
            ScriptErrorDetailsList = scriptErrorDetailsList;
            Hash = string.Empty;
        }
    }
}
