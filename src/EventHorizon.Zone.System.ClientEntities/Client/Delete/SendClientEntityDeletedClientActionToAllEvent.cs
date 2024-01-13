namespace EventHorizon.Zone.System.ClientEntities.Client.Delete;

using EventHorizon.Zone.Core.Events.Client.Generic;
using EventHorizon.Zone.System.ClientEntities.Model.Client;

public static class SendClientEntityDeletedClientActionToAllEvent
{
    public static ClientActionGenericToAllEvent Create(
        ClientEntityDeletedClientActionData data
    ) => new(
        "SERVER_CLIENT_ENTITY_DELETED_CLIENT_ACTION_EVENT",
        data
    );
}
