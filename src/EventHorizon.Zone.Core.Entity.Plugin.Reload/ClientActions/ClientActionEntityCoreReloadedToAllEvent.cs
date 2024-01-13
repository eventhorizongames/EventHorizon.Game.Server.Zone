namespace EventHorizon.Zone.Core.Entity.Plugin.Reload.ClientActions;

using EventHorizon.Zone.Core.Entity.Plugin.Reload.Model.Client;
using EventHorizon.Zone.Core.Events.Client.Generic;

public static class ClientActionEntityCoreReloadedToAllEvent
{
    public static ClientActionGenericToAllEvent Create(
        EntityCoreReloadedEventData data
    ) => new(
        "Entity.ENTITY_CORE_RELOADED",
        data
    );
}
