namespace EventHorizon.Zone.System.ClientEntities.Client
{
    using EventHorizon.Zone.Core.Events.Client.Generic;
    using EventHorizon.Zone.System.ClientEntities.Model.Client;

    public class SendClientEntityChangedClientActionToAllEvent
    {
        public static ClientActionGenericToAllEvent Create(
            ClientEntityChangedClientActionData data
        ) => new ClientActionGenericToAllEvent(
            "SERVER_CLIENT_ENTITY_CHANGED_CLIENT_ACTION_EVENT",
            data
        );
    }
}