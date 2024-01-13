namespace EventHorizon.Zone.System.Client.Scripts.Model;

public class ClientScriptsSettings
{
    public string CompilerSubProcessDirectory { get; }
    public string CompilerSubProcess { get; }

    public ClientScriptsSettings(
        string compilerSubProcessDirectory,
        string compilerSubProcess
    )
    {
        CompilerSubProcessDirectory = compilerSubProcessDirectory;
        CompilerSubProcess = compilerSubProcess;
    }
}
