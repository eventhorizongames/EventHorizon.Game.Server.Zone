namespace EventHorizon.Zone.Core.Events.Entity.Client
{
    using EventHorizon.Zone.Core.Events.Client.Generic;
    using EventHorizon.Zone.Core.Model.Entity.Client;

    public static class ClientActionClientEntityStoppingToAllEvent
    {
        public static ClientActionGenericToAllEvent Create(
            EntityClientStoppingData data
        ) => new ClientActionGenericToAllEvent(
            "ClientEntityStopping",
            data
        );
    }
}