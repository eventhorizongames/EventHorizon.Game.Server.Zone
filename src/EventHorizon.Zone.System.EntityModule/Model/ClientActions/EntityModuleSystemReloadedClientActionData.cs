namespace EventHorizon.Zone.System.EntityModule.Model.ClientActions;

using EventHorizon.Zone.Core.Model.Client;
using EventHorizon.Zone.System.EntityModule.Model;

using global::System.Collections.Generic;

public class EntityModuleSystemReloadedClientActionData
    : IClientActionData
{
    public IEnumerable<EntityScriptModule> BaseEntityScriptModuleList { get; }
    public IEnumerable<EntityScriptModule> PlayerEntityScriptModuleList { get; }

    public EntityModuleSystemReloadedClientActionData(
        IEnumerable<EntityScriptModule> baseEntityScriptModuleList,
        IEnumerable<EntityScriptModule> playerEntityScriptModuleList
    )
    {
        BaseEntityScriptModuleList = baseEntityScriptModuleList;
        PlayerEntityScriptModuleList = playerEntityScriptModuleList;
    }
}
