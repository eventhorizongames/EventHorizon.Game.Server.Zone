namespace EventHorizon.Zone.System.Client.Scripts.Model.Client
{
    using EventHorizon.Zone.Core.Model.Client;

    using global::System.Collections.Generic;

    public struct ClientScriptsSystemReloadedClientActionData
        : IClientActionData
    {
        public IEnumerable<ClientScript> ClientScriptList { get; }

        public ClientScriptsSystemReloadedClientActionData(
            IEnumerable<ClientScript> clientScriptList
        )
        {
            ClientScriptList = clientScriptList;
        }
    }
}
