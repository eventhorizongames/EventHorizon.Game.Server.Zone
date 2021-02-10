namespace EventHorizon.Zone.System.Server.Scripts.Model
{
    public class ServerScriptsSettings
    {
        public string CompilerSubProcessDirectory { get; }
        public string CompilerSubProcess { get; }

        public ServerScriptsSettings(
            string compilerSubProcessDirectory, 
            string compilerSubProcess
        )
        {
            CompilerSubProcessDirectory = compilerSubProcessDirectory;
            CompilerSubProcess = compilerSubProcess;
        }
    }
}
