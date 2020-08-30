namespace EventHorizon.Zone.System.Client.Scripts.Api
{
    public interface ClientScriptsState
    {
        string Hash { get; }
        string ScriptAssembly { get; }
        void SetAssembly(
            string hash,
            string scriptAssembly
        );
    }
}
