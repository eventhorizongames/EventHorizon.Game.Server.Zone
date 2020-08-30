namespace EventHorizon.Zone.System.Client.Scripts.Model.Client
{
    using EventHorizon.Zone.Core.Model.Client;

    public struct ClientScriptsAssemblyChangedClientActionData
        : IClientActionData
    {
        public string Hash { get; }
        public string ScriptAssembly { get; }

        public ClientScriptsAssemblyChangedClientActionData(
            string hash,
            string scriptAssembly
        )
        {
            Hash = hash;
            ScriptAssembly = scriptAssembly;
        }
    }
}
