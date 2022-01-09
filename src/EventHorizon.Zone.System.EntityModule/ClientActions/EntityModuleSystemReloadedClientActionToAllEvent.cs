namespace EventHorizon.Zone.System.EntityModule.ClientActions;

using EventHorizon.Zone.Core.Events.Client.Generic;
using EventHorizon.Zone.System.EntityModule.Model.ClientActions;

public static class EntityModuleSystemReloadedClientActionToAllEvent
{
    public static ClientActionGenericToAllEvent Create(
        EntityModuleSystemReloadedClientActionData data
    ) => new(
        "ENTITY_MODULE_SYSTEM_RELOADED_CLIENT_ACTION_EVENT",
        data
    );
}
