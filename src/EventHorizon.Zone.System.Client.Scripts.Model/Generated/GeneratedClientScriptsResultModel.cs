namespace EventHorizon.Zone.System.Client.Scripts.Model.Generated
{
    public class GeneratedClientScriptsResultModel
    {
        public static string GENERATED_FILE_NAME => "GeneratedClientScriptsResult.json";

        public bool Success { get; set; }
        public string? ErrorCode { get; set; }
        public string Hash { get; set; } = string.Empty;
        public string ScriptAssembly { get; set; } = string.Empty;
    }
}
