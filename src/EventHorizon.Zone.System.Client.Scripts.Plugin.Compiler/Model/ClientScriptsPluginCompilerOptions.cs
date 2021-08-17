namespace EventHorizon.Zone.System.Client.Scripts.Plugin.Compiler.Model
{
    public class ClientScriptsPluginCompilerOptions
    {
        public string SdkPackage { get; set; } = string.Empty;
        public string SdkPackageVersion { get; set; } = string.Empty;
        public bool IncludePrerelease { get; set; }
        public string NuGetFeed { get; set; } = string.Empty;
    }
}
