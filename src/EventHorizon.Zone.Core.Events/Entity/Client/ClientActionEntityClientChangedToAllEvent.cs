namespace EventHorizon.Zone.Core.Events.Entity.Client
{
    using EventHorizon.Zone.Core.Events.Client.Generic;
    using EventHorizon.Zone.Core.Model.Entity.Client;

    public static class ClientActionEntityClientChangedToAllEvent
    {
        public static ClientActionGenericToAllEvent Create(
            EntityChangedData data
        ) => new(
            "EntityClientChanged",
            data
        );
    }
}
