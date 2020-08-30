namespace EventHorizon.Zone.System.Client.Scripts.State
{
    using EventHorizon.Zone.System.Client.Scripts.Api;

    public class InMemoryClientScriptsState
        : ClientScriptsState
    {
        public string Hash { get; private set; } = string.Empty;
        public string ScriptAssembly { get; private set; } = string.Empty;

        public void SetAssembly(
            string hash,
            string scriptAssembly
        )
        {
            Hash = hash;
            ScriptAssembly = scriptAssembly;
        }
    }
}
