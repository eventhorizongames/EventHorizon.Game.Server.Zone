namespace EventHorizon.Zone.System.ServerModule.Model.Client
{
    using EventHorizon.Zone.Core.Model.Client;

    using global::System.Collections.Generic;

    public class ServerModuleSystemReloadedClientActionData : IClientActionData
    {
        public IEnumerable<ServerModuleScripts> ServerModuleScriptList { get; }

        public ServerModuleSystemReloadedClientActionData(
            IEnumerable<ServerModuleScripts> serverModuleScriptList
        )
        {
            ServerModuleScriptList = serverModuleScriptList;
        }
    }
}
