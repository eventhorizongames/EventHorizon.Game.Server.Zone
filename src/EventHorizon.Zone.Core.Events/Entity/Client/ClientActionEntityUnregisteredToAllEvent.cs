namespace EventHorizon.Zone.Core.Events.Entity.Client
{
    using EventHorizon.Zone.Core.Events.Client.Generic;
    using EventHorizon.Zone.Core.Model.Entity.Client;

    public static class ClientActionEntityUnregisteredToAllEvent
    {
        public static ClientActionGenericToAllEvent Create(
            EntityUnregisteredData data
        ) => new ClientActionGenericToAllEvent(
            "EntityUnregistered",
            data
        );
    }
}