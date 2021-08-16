namespace EventHorizon.Zone.System.Server.Scripts.Model.Generated
{
    public class GeneratedServerScriptsResultModel
    {
        public static string SCRIPTS_RESULT_FILE_NAME => "GeneratedServerScriptsResult.json";
        public static string SCRIPTS_ASSEMBLY_FILE_NAME => "Server_Scripts.dll";

        public bool Success { get; set; }
        public string ErrorCode { get; set; }
        public string Hash { get; set; } = string.Empty;
    }
}
