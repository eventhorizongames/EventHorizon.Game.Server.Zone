namespace EventHorizon.Zone.Core.Events.Map.Client
{
    using EventHorizon.Zone.Core.Events.Client.Generic;
    using EventHorizon.Zone.Core.Model.Map.Client;

    public static class ClientActionCoreMapLoadedToAllEvent
    {
        public static ClientActionGenericToAllEvent Create(
            CoreMapLoadedClientData data
        ) => new ClientActionGenericToAllEvent(
            "Core.Map.Created",
            data
        );
    }
}
