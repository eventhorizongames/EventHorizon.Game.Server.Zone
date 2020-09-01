namespace EventHorizon.Zone.System.Client.Scripts.Plugin.Compiler.Model
{
    using System;

    public class ClientScriptsPluginCompilerOptions
    {
        public string SdkPackage { get; set; }
        public string SdkPackageVersion { get; set; }
        public bool IncludePrerelease { get; set; }
    }
}
