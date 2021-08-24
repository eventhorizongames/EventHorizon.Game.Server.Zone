namespace EventHorizon.Zone.Core.Events.Entity.Client
{
    using EventHorizon.Zone.Core.Events.Client.Generic;
    using EventHorizon.Zone.Core.Model.Entity.Client;

    public static class ClientActionEntityClientMoveToAllEvent
    {
        public static ClientActionGenericToAllEvent Create(
            EntityClientMoveData data
        ) => new(
            "EntityClientMove",
            data
        );
    }
}
