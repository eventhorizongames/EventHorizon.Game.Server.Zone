namespace EventHorizon.Zone.Core.Entity.Plugin.Reload.ClientActions
{
    using EventHorizon.Zone.Core.Entity.Plugin.Reload.Model.Client;
    using EventHorizon.Zone.Core.Events.Client.Generic;

    public static class ClientActionCoreEntityReloadedToAllEvent
    {
        public static ClientActionGenericToAllEvent Create(
            CoreEntityReloadedEventData data
        ) => new(
            "Entity.CORE_ENTITY_RELOADED",
            data
        );
    }
}
