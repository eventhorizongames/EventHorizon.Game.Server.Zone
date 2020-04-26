namespace EventHorizon.Zone.Core.Events.Entity.Client
{
    using EventHorizon.Zone.Core.Events.Client.Generic;
    using EventHorizon.Zone.Core.Model.Entity.Client;

    public static class ClientActionEntityRegisteredToAllEvent
    {
        public static ClientActionGenericToAllEvent Create(
            EntityRegisteredData data
        ) => new ClientActionGenericToAllEvent(
            "EntityRegistered",
            data
        );
    }
}